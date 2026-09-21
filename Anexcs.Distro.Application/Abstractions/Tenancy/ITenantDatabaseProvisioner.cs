namespace Anexcs.Distro.Application.Abstractions.Tenancy;

public interface ITenantDatabaseProvisioner
{
    Task<TenantDatabaseProvisionResult> ProvisionAsync(
        Guid tenantId,
        CancellationToken cancellationToken);
}