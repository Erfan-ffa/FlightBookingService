using Application.Features.Passengers.Models;
using Application.Utils;
using MediatR;

namespace Application.Features.Bookings.Book;

public record CreateBookingRequest(long FlightId, PassengerModel Passenger) : IRequest<ApiResponse<CreateBookingResponse>>;