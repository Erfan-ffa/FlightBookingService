using Domain.Common;

namespace Domain.Entities;

public class Booking : AuditableEntity
{
    public long FlightId { get; set; }
    public long PassengerId { get; set; }
    public DateTime BookingDate { get; set; }
    public int SeatNumber { get; set; }
    
    public Flight Flight { get; set; }
    public Passenger Passenger { get; set; }

    private Booking()
    {
    }

    public static Booking Create(long flightId, long passengerId, int seatNumber)
    {
        return new Booking
        {
            FlightId = flightId,
            PassengerId = passengerId,
            SeatNumber = seatNumber,
            BookingDate = DateTime.Now
        };
    }
}