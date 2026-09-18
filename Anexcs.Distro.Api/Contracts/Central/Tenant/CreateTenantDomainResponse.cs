namespace Api.Contracts.Central.Tenant;

public record CreateTenantDomainResponse
{
    public Guid Id { get; init; }
    public string Domain { get; init; } = null!;
    public Guid TenantId { get; init; }
    public DateTime CreatedAtUtc { get; init; }
}