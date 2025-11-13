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
}