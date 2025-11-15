using Application.Features.Passengers.Models;
using Application.Utils;
using MediatR;

namespace Application.Features.Bookings.Book;

public record BookingRequest(long FlightId, PassengerModel Passenger) : IRequest<ApiResponse<BookingResponse>>;