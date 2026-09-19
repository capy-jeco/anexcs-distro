using Anexcs.Distro.Application.Abstractions.Persistence.Central;
using Microsoft.EntityFrameworkCore;

namespace Anexcs.Distro.Infrastructure.Persistence.Central.Repositories;

public sealed class TenantRepository(CentralDbContext context) : ITenantRepository
{
    public async Task AddAsync(
        Domain.Entities.Central.Tenant tenant,
        CancellationToken cancellationToken)
    {
        await context.Tenants.AddAsync(
            tenant,
            cancellationToken);
    }

    public async Task<Domain.Entities.Central.Tenant?> GetByDomainAsync(
        string domain,
        CancellationToken cancellationToken)
    {
        return await context.Tenants
            .Include(t => t.Domains)
            .AsNoTracking()
            .FirstOrDefaultAsync(
                t => t.Domains.Any(d => d.Domain == domain),
                cancellationToken);
    }
}