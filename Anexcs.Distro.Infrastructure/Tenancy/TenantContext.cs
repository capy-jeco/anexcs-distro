using Anexcs.Distro.Application.Abstractions.Tenancy;

namespace Anexcs.Distro.Infrastructure.Tenancy;

public class TenantContext : ITenantContext
{
    private Guid? _tenantId;

    public bool IsResolved => _tenantId.HasValue;

    public Guid TenantId => _tenantId
                            ?? throw new InvalidOperationException(
                                "No tenant has been resolved for the current request.");

    public void SetTenant(Guid tenantId)
    {
        if (_tenantId.HasValue)
        {
            throw new InvalidOperationException(
                "Tenant has already been set for this request and cannot be changed.");
        }

        _tenantId = tenantId;
    }
}