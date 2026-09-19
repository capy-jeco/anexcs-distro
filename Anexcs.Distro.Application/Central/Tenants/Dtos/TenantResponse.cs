namespace Anexcs.Distro.Application.Central.Tenants.Dtos;

public record TenantResponse(Guid Id, string Data, IEnumerable<string> Domains, DateTime CreatedAtUtc);