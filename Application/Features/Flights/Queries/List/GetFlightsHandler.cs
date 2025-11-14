using System.Xml;
using Application.Contracts.Repositories;
using Application.Features.Flights.Models;
using Application.Utils;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace Application.Features.Flights.Queries.List;

public class GetFlightsHandler(IUnitOfWork uow, IDistributedCache cache, ILogger<GetFlightsHandler> logger)
    : IRequestHandler<GetFlightsQuery, ApiResponse<GetFlightsResponse>>
{
    private static string FailureLogTemplate = "[GetFlightsFailure]-{}";
    
    public async Task<ApiResponse<GetFlightsResponse>> Handle(GetFlightsQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = GetCacheKey(request);
        var cachedFlights = await cache.GetAsync<List<FlightDto>>(cacheKey, token: cancellationToken);
        if (cachedFlights is not null)
        {
            var response = ConvertToResponse(cachedFlights);
            return ApiResponse<GetFlightsResponse>.Ok(response);
        }

        var flights = await uow.Flights.GetAllAsync(request.Origin, request.Destination, request.DepartureDate, cancellationToken);
        if(flights.Count == 0)
            return ApiResponse<GetFlightsResponse>.Ok(new GetFlightsResponse([]));

        var flightsDto = flights.Select(FlightDto.FromFlight).ToList();
        await CacheFlightsAsync(flightsDto, cacheKey , cancellationToken);
        
        var filteredFlights = flights
            .When(request.ArrivalDate.HasValue, x => DateOnly.FromDateTime(x.ArrivalTime) == request.ArrivalDate)
            .When(request.AvailableSeats.HasValue, x => x.AvailableSeats == request.AvailableSeats)
            .Select(FlightDto.FromFlight)
            .ToList();
        
        return ApiResponse<GetFlightsResponse>.Ok(ConvertToResponse(filteredFlights));
    }

    private static GetFlightsResponse ConvertToResponse(List<FlightDto> cachedFlights) => new(cachedFlights);

    private string GetCacheKey(GetFlightsQuery request)
        => string.Format(FlightKeys.AvailableKeyFormat, request.Origin, request.Destination, request.DepartureDate);
    
    private async Task CacheFlightsAsync(List<FlightDto> flights, string cacheKey, CancellationToken cancellationToken)
    {
        try
        {
            var options = new DistributedCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromDays(5));
            await cache.SetAsync(cacheKey, flights, token: cancellationToken, options: options);
        }
        catch (Exception e)
        {
            logger.LogError(FailureLogTemplate, $"Caching Flights Failed. Details: {e.Message}");
        }
    }
}