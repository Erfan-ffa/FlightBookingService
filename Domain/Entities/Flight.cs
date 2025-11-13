using Domain.Common;

namespace Domain.Entities;

public class Flight : AuditableEntity
{
    public string FlightNumber { get; set; }
    public string Origin { get; set; }
    public string Destination { get; set; }
    public DateTime DepartureTime { get; set; }
    public DateTime ArrivalTime { get; set; }
    public int AvailableSeats { get; set; }
    public decimal Price { get; set; }
}