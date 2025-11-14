using Application.Utils;
using MediatR;

namespace Application.Features.Flights.Commands.Create;

public record CreateFlightRequest(string FlightNumber, string Origin, string Destination,
    DateTime DepartureTime, DateTime ArrivalTime, int AvailableSeats, decimal Price) : IRequest<ApiResponse<CreateFlightResponse>>;