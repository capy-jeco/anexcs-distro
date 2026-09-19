using Anexcs.Distro.Application.Abstractions.Persistence.Tenant;
using Microsoft.EntityFrameworkCore;

namespace Anexcs.Distro.Infrastructure.Persistence.Tenant;

public class TenantDbContext : DbContext, ITenantUnitOfWork
{
    public TenantDbContext(
        DbContextOptions<TenantDbContext> options)
        : base(options)
    {
    }
}