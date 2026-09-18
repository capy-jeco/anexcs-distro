using Anexcs.Distro.Application.Abstractions.Persistence;
using Anexcs.Distro.Infrastructure.Persistence.Central;

namespace Anexcs.Distro.Infrastructure.Persistence.Repositories.Central;

public sealed class TenantRepository : ITenantRepository
{
    private readonly CentralDbContext _context;

    public TenantRepository(CentralDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(
        Domain.Entities.Central.Tenant tenant,
        CancellationToken cancellationToken)
    {
        await _context.Tenants.AddAsync(
            tenant,
            cancellationToken);
    }
}