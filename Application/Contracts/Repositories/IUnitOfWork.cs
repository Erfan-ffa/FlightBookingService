namespace Application.Contracts.Repositories;

public interface IUnitOfWork
{
    IFlightRepository Flights { get; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}