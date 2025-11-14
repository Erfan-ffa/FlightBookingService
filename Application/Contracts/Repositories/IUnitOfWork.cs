namespace Application.Contracts.Repositories;

public interface IUnitOfWork
{
    IFlightRepository Flights { get; }
    IBookingRepository  Bookings { get; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}