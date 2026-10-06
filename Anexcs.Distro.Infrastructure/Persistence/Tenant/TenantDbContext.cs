using Anexcs.Distro.Application.Abstractions.Persistence.Tenant;
using Anexcs.Distro.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Anexcs.Distro.Infrastructure.Persistence.Tenant;

public class TenantDbContext : IdentityDbContext<TenantIdentityUser, IdentityRole<Guid>, Guid>, ITenantUnitOfWork
{
    public TenantDbContext(
        DbContextOptions<TenantDbContext> options)
        : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}