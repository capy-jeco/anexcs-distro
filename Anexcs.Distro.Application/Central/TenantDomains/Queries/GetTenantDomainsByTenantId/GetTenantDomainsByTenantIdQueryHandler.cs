using Anexcs.Distro.Application.Abstractions.Persistence.Central;
using Anexcs.Distro.Application.Central.TenantDomains.Dtos;

namespace Anexcs.Distro.Application.Central.TenantDomains.Queries.GetTenantDomainsByTenantId;

public sealed class GetTenantDomainsByTenantIdQueryHandler(
    ITenantDomainRepository tenantDomainRepository)
{
    public async Task<IReadOnlyList<TenantDomainResponse>> Handle(
        GetTenantDomainsByTenantIdQuery request,
        CancellationToken cancellationToken)
    {
        var domains = await tenantDomainRepository.GetByTenantId(
            request.TenantId,
            cancellationToken);

        return
        [
            .. domains
                .Select(domain => new TenantDomainResponse(
                    domain.Id,
                    domain.TenantId,
                    domain.Domain))
        ];
    }
}