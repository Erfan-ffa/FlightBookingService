using Domain.Entities;

namespace Application.Contracts.Repositories;

public interface IBookingRepository
{
    Task<List<Booking>> GetByFlightIdAsync(long id, CancellationToken cancellationToken);
    HashSet<int> GetBookedSeatNumbers(long id, CancellationToken cancellationToken);
    Task<bool> SeatAlreadyBooked(long flightId, int seatNumber, CancellationToken cancellationToken);
    void Add(Booking booking);
}