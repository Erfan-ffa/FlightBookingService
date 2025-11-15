using System.Net;
using Application.ApiMessages;
using Application.Contracts;
using Application.Contracts.Repositories;
using Application.Utils;
using Domain.Entities;
using MediatR;

namespace Application.Features.Flights.Commands.Create;

public class CreateFlightHandler(IUnitOfWork uow, ICacheService cacheService)
    : IRequestHandler<CreateFlightRequest, ApiResponse<CreateFlightResponse>>
{
    public async Task<ApiResponse<CreateFlightResponse>> Handle(CreateFlightRequest request,
        CancellationToken cancellationToken)
    {
        var flightExists = await uow.Flights.ExistsAsync(request.FlightNumber, cancellationToken);
        if (flightExists)
            return ApiResponse<CreateFlightResponse>.BadRequest(FlightMessages.AlreadyExists);

        var flight = Flight.Create(request.FlightNumber, request.Origin, request.Destination, request.DepartureTime,
            request.ArrivalTime, request.AvailableSeats, request.Price);
        
        uow.Flights.Add(flight);
        await uow.SaveChangesAsync(cancellationToken);

        await CleanFlightsCache(request);
        await AllocateSeatsForFlightAsync(flight.Id, flight.AvailableSeats);
        
        var result = new CreateFlightResponse
        {
            Id = flight.Id
        };
        
        return ApiResponse<CreateFlightResponse>.Ok(result);
    }

    private async Task CleanFlightsCache(CreateFlightRequest request)
    {
        await cacheService.RemoveAsync(GetCacheKey(request.Origin, request.Destination, request.DepartureTime));
    }

    private string GetCacheKey(string origin, string destination, DateTime departureTime)
        => string.Format(CacheKeys.AvailableKeyFormat, origin, destination, DateOnly.FromDateTime(departureTime));

    private async Task AllocateSeatsForFlightAsync(long flightId, int availableSeats)
    {
        var seats = Enumerable.Range(1, availableSeats).ToList();
        await cacheService.BulkPushAsync(string.Format(CacheKeys.FlightSeatsKeyFormat, flightId), seats);
    }
}