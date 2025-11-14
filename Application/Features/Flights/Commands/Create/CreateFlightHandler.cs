using System.Net;
using Application.Contracts.Repositories;
using Application.Utils;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;

namespace Application.Features.Flights.Commands.Create;

public class CreateFlightHandler(IUnitOfWork uow, IDistributedCache cache)
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

        await CleanFlightsCache(request, cancellationToken);
        
        var result = new CreateFlightResponse
        {
            Id = flight.Id
        };
        
        return ApiResponse<CreateFlightResponse>.Ok(result);
    }

    private async Task CleanFlightsCache(CreateFlightRequest request, CancellationToken cancellationToken)
    {
        await cache.RemoveAsync(GetCacheKey(request.Origin, request.Destination, request.DepartureTime), cancellationToken);
    }

    private string GetCacheKey(string origin, string destination, DateTime departureTime)
        => string.Format(FlightKeys.AvailableKeyFormat, origin, destination, DateOnly.FromDateTime(departureTime));
}