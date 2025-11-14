using Application.Contracts.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class BookingRepository(AppDbContext dbContext) : IBookingRepository
{

    public async Task<List<Booking>> GetByFlightIdAsync(long id, CancellationToken cancellationToken)
        => await dbContext.Bookings.Include(x => x.Passenger)
            .Where(x => x.FlightId == id).ToListAsync(cancellationToken);
}