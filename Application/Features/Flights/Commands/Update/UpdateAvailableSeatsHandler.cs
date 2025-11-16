using System.Net;
using Application.ApiMessages;
using Application.Contracts;
using Application.Contracts.Repositories;
using Application.Utils;
using Domain.Entities;
using MediatR;

namespace Application.Features.Flights.Commands.Update;

public class UpdateAvailableSeatsHandler(IUnitOfWork uow, ICacheService cacheService) 
    : IRequestHandler<UpdateAvailableSeatsRequest, ApiResponse>
{
    public async Task<ApiResponse> Handle(UpdateAvailableSeatsRequest request, CancellationToken cancellationToken)
    {
        var flight = await uow.Flights.GetWithLockAsync(request.FlightId, cancellationToken);
        if(flight is null)
            return ApiResponse.Error(FlightMessages.NotFound, HttpStatusCode.NotFound);

        if(request.AvailableSeats < flight.AvailableSeats)
            return ApiResponse.BadRequest(FlightMessages.CanNotDecreaseAvailableSeats);
        
        flight.UpdateAvailableSeats(request.AvailableSeats);
        await CleanFlightSeatsCacheAsync(flight);
        await uow.SaveChangesAsync(cancellationToken);
        
        return ApiResponse.Ok();
    }

    private async Task CleanFlightSeatsCacheAsync(Flight flight)
    {
        var seatsCache = string.Format(CacheKeys.FlightSeatsKeyFormat, flight.Id);
        await cacheService.RemoveAsync(seatsCache);
    }
}