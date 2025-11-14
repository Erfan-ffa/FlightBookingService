using Application.Utils;
using MediatR;

namespace Application.Features.Bookings.Query.ListByFlightId;

public record GetBookingsByFlightIdRequest(long FlightId) : IRequest<ApiResponse<GetBookingsByFlightIdResponse>>;