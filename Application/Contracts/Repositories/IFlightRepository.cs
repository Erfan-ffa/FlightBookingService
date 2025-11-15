using System.Linq.Expressions;
using Domain.Entities;

namespace Application.Contracts.Repositories;

public interface IFlightRepository
{
    void Add(Flight flight);
    Task<bool> ExistsAsync(string flightNumber, CancellationToken cancellationToken);
    Task<List<Flight>> GetAllAsync(string origin, string destination, DateOnly departureDate,
        CancellationToken cancellationToken);

    Task<Flight?> GetByIdAsync(long id, CancellationToken cancellationToken);
    Task<int> DecreaseAvailableSeatsAsync(long flightId, CancellationToken cancellationToken);
    Task<Flight?> GetWithLockAsync(long id, CancellationToken cancellationToken);
}