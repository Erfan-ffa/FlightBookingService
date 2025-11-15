using Application.Contracts.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class BookingRepository(AppDbContext dbContext) : IBookingRepository
{
    public void Add(Booking booking)
    {
        dbContext.Bookings.Add(booking);
    }

    public async Task<List<Booking>> GetByFlightIdAsync(long id, CancellationToken cancellationToken)
        => await dbContext.Bookings.Include(x => x.Passenger)
            .Where(x => x.FlightId == id).ToListAsync(cancellationToken);

    public HashSet<int> GetBookedSeatNumbers(long id, CancellationToken cancellationToken)
    {
        return dbContext.Bookings
            .Where(x => x.Id == id)
            .Select(x => x.SeatNumber)
            .ToHashSet();
    }

    public async Task<bool> SeatAlreadyBooked(long flightId, int seatNumber, CancellationToken cancellationToken)
        => await dbContext.Bookings.AnyAsync(x => x.FlightId == flightId && x.SeatNumber == seatNumber,
            cancellationToken);
}