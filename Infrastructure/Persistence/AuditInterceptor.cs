using Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Infrastructure.Persistence;

public class AuditInterceptor : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData, 
        InterceptionResult<int> result)
    {
        UpdateAuditFields(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData, 
        InterceptionResult<int> result, 
        CancellationToken cancellationToken = default)
    {
        UpdateAuditFields(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void UpdateAuditFields(DbContext? context)
    {
        if (context is null)
            return;

        var entries = context.ChangeTracker.Entries()
            .Where(e => e is { Entity: IEntity, State: EntityState.Added or EntityState.Modified });
        
        var now = DateTime.UtcNow;

        foreach (var entry in entries)
        {
            var createdAtProp = entry.Property(nameof(AuditableEntity.CreatedAt));
            var modifiedAtProp = entry.Property(nameof(AuditableEntity.ModifiedAt));

            switch (entry.State)
            {
                case EntityState.Added:
                    createdAtProp.CurrentValue = now;
                    modifiedAtProp.CurrentValue = now;
                    break;

                case EntityState.Modified:
                    modifiedAtProp.CurrentValue = now;
                    createdAtProp.IsModified = false;
                    break;
            }
        }
    }
}
