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

    
    private readonly List<Booking> _bookings = new();
    public IReadOnlyCollection<Booking> Bookings => _bookings.AsReadOnly();
    
    private Flight()
    {
    }

    public static Flight Create(string flightNumber, string origin, string destination, DateTime departureTime,
        DateTime arrivalTime, int availableSeats, decimal price)
    {
        return new Flight
        {
            FlightNumber = flightNumber,
            Origin = origin,
            Destination = destination,
            DepartureTime = departureTime,
            ArrivalTime = arrivalTime,
            AvailableSeats = availableSeats,
            Price = price
        };
    }

    public void DecreaseAvailableSeats()
    {
        AvailableSeats -= 1;
    }
}