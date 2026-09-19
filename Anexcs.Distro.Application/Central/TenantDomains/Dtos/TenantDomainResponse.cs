namespace Anexcs.Distro.Application.Central.TenantDomains.Dtos;

public record TenantDomainResponse(
    Guid Id, 
    Guid TenantId,
    string Domain);