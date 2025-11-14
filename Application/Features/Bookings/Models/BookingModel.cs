using Domain.Entities;

namespace Application.Features.Bookings.Models;

public class BookingModel
{
    public string PassengerFullName { get; set; }
    public DateTime BookingDate { get; set; }
    public int SeatNumber { get; set; }

    public static BookingModel FromBooking(Booking booking)
    {
        return new BookingModel
        {
            BookingDate = booking.BookingDate,
            SeatNumber =  booking.SeatNumber,
            PassengerFullName = booking.Passenger.Fullname
        };
    }
}