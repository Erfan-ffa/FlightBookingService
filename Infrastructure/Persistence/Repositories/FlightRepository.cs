using Application.Contracts.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class FlightRepository(AppDbContext dbContext) : IFlightRepository
{
    public void Add(Flight flight)
    {
        dbContext.Flights.Add(flight);
    }

    public async Task<bool> ExistsAsync(string flightNumber, CancellationToken cancellationToken)
        => await dbContext.Flights.AnyAsync(x => x.FlightNumber == flightNumber, cancellationToken);
}