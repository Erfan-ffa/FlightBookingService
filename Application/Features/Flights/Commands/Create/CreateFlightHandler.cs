using System.Net;
using Application.Contracts.Repositories;
using Application.Utils;
using Domain.Entities;
using MediatR;

namespace Application.Features.Flights.Commands.Create;

public class CreateFlightHandler(IUnitOfWork uow)
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

        var result = new CreateFlightResponse
        {
            Id = flight.Id
        };
        
        return ApiResponse<CreateFlightResponse>.Ok(result);
    }
}