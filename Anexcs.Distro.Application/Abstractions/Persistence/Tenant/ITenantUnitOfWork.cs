namespace Anexcs.Distro.Application.Abstractions.Persistence.Tenant;

public interface ITenantUnitOfWork
{
    Task<int> SaveChangesAsync(
        CancellationToken cancellationToken);
}