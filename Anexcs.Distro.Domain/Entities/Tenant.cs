namespace Anexcs.Distro.Domain.Entities;

public class Tenant
{
    public Guid Id { get; private set; }
    public string Data { get; private set; } = "{}"; // JSON blob
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
}