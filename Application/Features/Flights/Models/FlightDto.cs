using Domain.Entities;

namespace Application.Features.Flights.Models;

public class FlightDto
{
    public string FlightNumber { get; set; }
    public string Origin { get; set; }
    public string Destination { get; set; }
    public int AvailableSeats { get; set; }
    public decimal Price { get; set; }
    
    public static FlightDto FromFlight(Flight flight)
    {
        return new FlightDto
        {
            FlightNumber = flight.FlightNumber,
            Origin = flight.Origin,
            Destination = flight.Destination,
            AvailableSeats = flight.AvailableSeats,
            Price = flight.Price,
        };
    }
}