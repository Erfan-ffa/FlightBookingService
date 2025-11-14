using Domain.Entities;

namespace Application.Contracts.Repositories;

public interface IBookingRepository
{
    Task<List<Booking>> GetByFlightIdAsync(long id, CancellationToken cancellationToken);
}