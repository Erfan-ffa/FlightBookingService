using System.Net;
using Application.ApiMessages;
using Application.Contracts;
using Application.Contracts.Repositories;
using Application.Features.Flights;
using Application.Utils;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using RedLockNet;

namespace Application.Features.Bookings.Book;

public class CreateBookingHandler : IRequestHandler<CreateBookingRequest, ApiResponse<CreateBookingResponse>>
{
    private readonly IUnitOfWork _uow;
    private readonly ICacheService _cacheService;
    private readonly IDistributedLockFactory _redLock;
    private readonly ILogger<CreateBookingHandler> _logger;

    public CreateBookingHandler(IUnitOfWork uow, ICacheService cacheService, IDistributedLockFactory redLock, ILogger<CreateBookingHandler> logger)
    {
        _uow = uow;
        _cacheService = cacheService;
        _redLock = redLock;
        _logger = logger;
    }

    public async Task<ApiResponse<CreateBookingResponse>> Handle(CreateBookingRequest request, CancellationToken cancellationToken)
    {
        var flight = await _uow.Flights.GetByIdAsync(request.FlightId, cancellationToken);
        if(flight is null)
            return ApiResponse<CreateBookingResponse>.BadRequest(FlightMessages.NotFound);
        
        var passengerId = GetFromToken();
        var seatResponse = await _cacheService.PopAsync<int?>(string.Format(CacheKeys.FlightSeatsKeyFormat, flight.Id));
        
        if (CouldNotAcquireSeat(seatResponse))
            return await BookUsingFallbackMechanismAsync(request, flight, cancellationToken);

        var seatId = seatResponse!.Value;
        if(NoAvailableSeatExists(seatId))
            return ApiResponse<CreateBookingResponse>.BadRequest(FlightMessages.SeatsNotAvailable);
            
        var booking = await CreateBookingAsync(request, cancellationToken, passengerId, seatId, flight);

        return ApiResponse<CreateBookingResponse>.Ok(new CreateBookingResponse(booking.Id));
    }
    
    private static bool CouldNotAcquireSeat(int? seatId) => seatId is null;
    
    private async Task<ApiResponse<CreateBookingResponse>> BookUsingFallbackMechanismAsync(CreateBookingRequest request,
        Flight flight,
        CancellationToken cancellationToken)
    {
        if(flight.AvailableSeats <= 0)
            return ApiResponse<CreateBookingResponse>.BadRequest(FlightMessages.SeatsNotAvailable);
            
        var isRedisHealthy = await _cacheService.PingAsync() is not null;
        if (isRedisHealthy)
            await RebuildStateInBackground(request.FlightId, cancellationToken);
            
        var bookingResult = await BookUsingDatabaseLockAsync(request, cancellationToken);
        await ClearFlightCacheAsync(flight);
        return bookingResult;
    }

    private async Task<ApiResponse<CreateBookingResponse>> BookUsingDatabaseLockAsync(CreateBookingRequest request, CancellationToken cancellationToken)
    {
        var flight = await _uow.Flights.GetWithLockAsync(request.FlightId, cancellationToken);
        flight!.DecreaseAvailableSeats();
        var seatId = await GetFreeSeatIdAsync(request.FlightId, cancellationToken);
        if (seatId == 0)
            return ApiResponse<CreateBookingResponse>.BadRequest(FlightMessages.SeatsNotAvailable);
        
        var booking = Booking.Create(request.FlightId, GetFromToken(), seatId);
        _uow.Bookings.Add(booking);
        await _uow.SaveChangesAsync(cancellationToken);

        return ApiResponse<CreateBookingResponse>.Ok(new CreateBookingResponse(booking.Id));
    }

    private async Task<Booking> CreateBookingAsync(CreateBookingRequest request, CancellationToken cancellationToken,
        int passengerId, int seatId, Flight flight)
    {
        var booking = Booking.Create(request.FlightId, passengerId, seatId);
        
        await _uow.Flights.DecreaseAvailableSeatsAsync(flight.Id, cancellationToken);
        _uow.Bookings.Add(booking);
        await _uow.SaveChangesAsync(cancellationToken);
        await ClearFlightCacheAsync(flight);
        return booking;
    }

    
    private async Task RebuildStateInBackground(long flightId, CancellationToken cancellationToken)
    {
        var key = string.Format(CacheKeys.BookingLockKeyFormat, flightId);
            
        var expiration = TimeSpan.FromSeconds(5);
        var waitTime = TimeSpan.FromSeconds(5);
        var retryCount = TimeSpan.FromSeconds(3);
            
        await using var _lock = await _redLock.CreateLockAsync(key, expiration, waitTime, retryCount, cancellationToken);
        if (!_lock.IsAcquired)
        {
            _logger.LogError("Acquiring lock for flight {flightId} Failed.", flightId);
            return;
        }

        var seatsCacheKey = string.Format(CacheKeys.FlightSeatsKeyFormat, flightId);
        var alreadyFilled = await _cacheService.KeyExistsAsync(seatsCacheKey);
        if(alreadyFilled)
            return;

        var freeSeatIds = await GetFreeSeatIdsAsync(flightId, cancellationToken);
        await _cacheService.BulkPushAsync(seatsCacheKey, freeSeatIds);
    }

    private async Task<int> GetFreeSeatIdAsync(long flightId, CancellationToken cancellationToken)
    {
        var freeSeatIds = await GetFreeSeatIdsAsync(flightId, cancellationToken);
        return freeSeatIds.Count == 0 ? 0 : freeSeatIds.First();
    }
    
    private async Task<List<int>> GetFreeSeatIdsAsync(long flightId, CancellationToken cancellationToken)
    {
        var bookings = _uow.Bookings.GetBookedSeatNumbers(flightId, cancellationToken);
        var flight = await _uow.Flights.GetByIdAsync(flightId, cancellationToken);
        var totalSeats = bookings.Count + flight!.AvailableSeats;
        var openSeatNumbers = Enumerable.Range(1, totalSeats).Except(bookings).ToList();
        
        return  openSeatNumbers;
    }

    private static bool NoAvailableSeatExists(int seatId) => seatId == 0;
    private static int GetFromToken()
        => 1;

    private async Task ClearFlightCacheAsync(Flight flight)
    {
        var availableKey = string.Format(CacheKeys.AvailableKeyFormat, flight.Origin, flight.Destination, DateOnly.FromDateTime(flight.DepartureTime));
        await _cacheService.RemoveAsync(availableKey);
    }
}