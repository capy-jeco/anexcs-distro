namespace Anexcs.Distro.Domain.Entities.Central;

public class TenantDomain
{
    public Guid Id { get; private set; }
    public string Domain { get; private set; } = null!;
    public Guid TenantId { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime? UpdatedAtUtc { get; private set; }
    public Tenant Tenant { get; init; } = null!;
    
    private TenantDomain() { }

    public TenantDomain(string domain, Guid tenantId)
    {
        Id = Guid.NewGuid();
        Domain = domain;
        TenantId = tenantId;
        CreatedAtUtc = DateTime.UtcNow;
    }
}