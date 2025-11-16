using Application.Features.Bookings.Models;

namespace Application.Features.Bookings.Query.ListByFlightId;

public class GetBookingsByFlightIdResponse(List<BookingModel> bookings)
{
    public List<BookingModel> BookingModels { get; } = bookings;
}