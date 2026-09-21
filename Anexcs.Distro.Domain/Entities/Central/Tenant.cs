using Anexcs.Distro.Domain.Enums;

namespace Anexcs.Distro.Domain.Entities.Central;

public class Tenant
{
    public Guid Id { get; private set; }
    public string Data { get; private set; } = "{}"; // JSON blob
    public TenantStatus Status { get; private set; }
    public string? DatabaseName { get; private set; }
    public string? ServerKey { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime? UpdatedAtUtc { get; private set; }

    public ICollection<TenantDomain> Domains { get; set; } = [];
    
    private Tenant() { }

    public Tenant(Guid id, string data)
    {
        Id = id;
        Data = data;
        CreatedAtUtc = DateTime.UtcNow;
    }
    
    public void AssignDatabase(string databaseName, string serverKey)
    {
        DatabaseName = databaseName;
        ServerKey = serverKey;
        UpdatedAtUtc = DateTime.UtcNow;
    }
    
    public void MarkAsProvisioning()
    {
        Status = TenantStatus.Provisioning;
    }

    public void Activate()
    {
        Status = TenantStatus.Active;
    }

    public void Suspend()
    {
        Status = TenantStatus.Suspended;
    }

    public void MarkProvisioningFailed()
    {
        Status = TenantStatus.ProvisioningFailed;
    }
}