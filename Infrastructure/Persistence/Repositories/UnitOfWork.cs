using Application.Contracts.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Persistence.Repositories;

public class UnitOfWork(AppDbContext dbContext, IServiceProvider serviceProvider) : IUnitOfWork
{
    private IFlightRepository? _flights;
    private IBookingRepository? _bookings;
    
    public IFlightRepository Flights => _flights ??= serviceProvider.GetRequiredService<IFlightRepository>();
    public IBookingRepository Bookings => _bookings ??= serviceProvider.GetRequiredService<IBookingRepository>();


    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.SaveChangesAsync(cancellationToken);
    }
}