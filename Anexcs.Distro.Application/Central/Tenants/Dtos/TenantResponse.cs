namespace Anexcs.Distro.Application.Tenants.Dtos;

public record TenantResponse(Guid Id, string Data, IEnumerable<string> Domains, DateTime CreatedAtUtc);