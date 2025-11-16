using Application.Utils;
using MediatR;

namespace Application.Features.Flights.Commands.Update;

public record UpdateAvailableSeatsRequest(long FlightId, int AvailableSeats) : IRequest<ApiResponse>;