using Anexcs.Distro.Application.Central.TenantDomains.Dtos;
using MediatR;

namespace Anexcs.Distro.Application.Central.TenantDomains.Queries.GetTenantDomainsByTenantId;

public sealed record GetTenantDomainsByTenantIdQuery(
    Guid TenantId)
    : IRequest<IReadOnlyList<TenantDomainResponse>>;