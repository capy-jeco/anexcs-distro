using Microsoft.EntityFrameworkCore;

namespace Anexcs.Distro.Infrastructure.Persistence.Tenant;

public class TenantDbContext : DbContext
{
    public TenantDbContext(
        DbContextOptions<TenantDbContext> options)
        : base(options)
    {
    }
}