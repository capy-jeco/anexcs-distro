namespace Api.Contracts.Central.Tenant;

public record CreateTenantDomainRequest(
    Guid Id, 
    string Domain, 
    Guid TenantId, 
    DateTime CreatedAtUtc);