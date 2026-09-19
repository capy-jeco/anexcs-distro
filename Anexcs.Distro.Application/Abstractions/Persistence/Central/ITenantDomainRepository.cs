using Anexcs.Distro.Domain.Entities.Central;

namespace Anexcs.Distro.Application.Abstractions.Persistence.Central;

public interface ITenantDomainRepository
{
    /// <summary>
    /// Gets the list of tenant domains by tenant id.
    /// </summary>
    /// <param name="tenantId">Tenant ID</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<IReadOnlyList<TenantDomain>> GetByTenantId(
        Guid tenantId,
        CancellationToken cancellationToken);
}