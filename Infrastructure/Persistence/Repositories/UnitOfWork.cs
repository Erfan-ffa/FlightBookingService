using Application.Contracts.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Persistence.Repositories;

public class UnitOfWork(AppDbContext dbContext, IServiceProvider serviceProvider) : IUnitOfWork
{
    private IFlightRepository? _flights;
    
    public IFlightRepository Flights => _flights ??= serviceProvider.GetRequiredService<IFlightRepository>();
    
    
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.SaveChangesAsync(cancellationToken);
    }
}