using Anexcs.Distro.Application.Abstractions.Persistence.Central;
using Anexcs.Distro.Application.Central.Tenants.Dtos;

namespace Anexcs.Distro.Application.Central.Tenants.Queries.GetTenantByDomain;

public sealed class GetTenantByDomainQueryHandler(
    ITenantRepository tenantRepository)
{
    public async Task<TenantResponse?> Handle(
        GetTenantByDomainQuery request,
        CancellationToken cancellationToken)
    {
        var domain = request.Domain
            .Trim()
            .ToLowerInvariant();

        var tenant = await tenantRepository.GetByDomainAsync(
            domain,
            cancellationToken);

        if (tenant is null)
        {
            return null;
        }

        return new TenantResponse(
            tenant.Id,
            tenant.Data,
            tenant.Domains.Select(d => d.Domain),
            tenant.CreatedAtUtc);
    }
}