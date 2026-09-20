using Anexcs.Distro.Application.Central.TenantDomains.Dtos;

namespace Anexcs.Distro.Application.Central.TenantDomains.Queries.GetTenantDomainsByTenantId;

public sealed record GetTenantDomainsByTenantIdQuery(
    Guid TenantId);