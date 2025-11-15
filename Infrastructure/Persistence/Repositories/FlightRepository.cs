using System.Linq.Expressions;
using Application.Contracts.Repositories;
using Application.Utils;
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

    public async Task<List<Flight>> GetAllAsync(string origin, string destination, DateOnly departureDate,
        CancellationToken cancellationToken)
    {
        return await dbContext.Flights
            .Where(x =>
                x.Origin.Equals(origin) &&
                x.Destination.Equals(destination) &&
                DateOnly.FromDateTime(x.DepartureTime) == departureDate &&
                x.DepartureTime > DateTime.Now)
            .OrderBy(x => x.DepartureTime)
            .ToListAsync(cancellationToken);
    }

    public async Task<Flight?> GetByIdAsync(long id, CancellationToken cancellationToken)
        => await dbContext.Flights.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<List<Flight>> GetAllAsync(Expression<Func<Flight, bool>> criteria,
        CancellationToken cancellationToken)
        => await dbContext.Flights.Where(criteria).ToListAsync(cancellationToken);

    public async Task<int> DecreaseAvailableSeatsAsync(long flightId, CancellationToken cancellationToken)
    {
        return await dbContext.Database.ExecuteSqlAsync(
            $"UPDATE Flights SET AvailableSeats = AvailableSeats - 1 WHERE Id = {flightId} AND AvailableSeats > 0",
            cancellationToken);
    }

    public async Task<Flight?> GetWithLockAsync(long id, CancellationToken cancellationToken)
    {
        return await dbContext.Flights
            .FromSqlInterpolated($@"
                        SELECT *
                        FROM Flights
                        WHERE id = {id}
                        FOR UPDATE NOWAIT
            ")
            .FirstOrDefaultAsync(cancellationToken);
    }
}