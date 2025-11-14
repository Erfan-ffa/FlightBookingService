using Domain.Entities;

namespace Application.Contracts.Repositories;

public interface IFlightRepository
{
    void Add(Flight flight);
    Task<bool> ExistsAsync(string flightNumber, CancellationToken cancellationToken);
}