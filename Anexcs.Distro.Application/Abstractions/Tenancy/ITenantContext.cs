namespace Anexcs.Distro.Application.Abstractions.Tenancy;

public interface ITenantContext
{
    bool IsResolved { get; }
    Guid TenantId { get; }
}