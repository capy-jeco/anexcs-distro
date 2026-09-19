using Anexcs.Distro.Application.Abstractions.Persistence.Central;
using Anexcs.Distro.Domain.Entities.Central;
using Microsoft.EntityFrameworkCore;

namespace Anexcs.Distro.Infrastructure.Persistence.Central.Repositories;

public sealed class TenantDomainRepository(
    CentralDbContext context) 
    : ITenantDomainRepository
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public async Task<IReadOnlyList<TenantDomain>> GetByTenantId(
        Guid tenantId,
        CancellationToken cancellationToken)
    {
        return await context.TenantDomains
            .AsNoTracking()
            .Where(d => d.TenantId == tenantId)
            .OrderBy(d => d.Domain)
            .ToListAsync(cancellationToken);
    }
}