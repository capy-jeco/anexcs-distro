using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Anexcs.Distro.Infrastructure.Persistence.Tenant;

public class TenantDbContextFactory : IDesignTimeDbContextFactory<TenantDbContext>
{
    public TenantDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<TenantDbContext>();

        // Design-time only — used solely for `dotnet ef migrations add`.
        // Never reached by a real request; the runtime connection is
        // always resolved dynamically via TenantConnectionStringResolver.
        optionsBuilder.UseNpgsql(
            "Host=localhost;Port=5432;Database=anexcs_tenant_design_time;Username=postgres;Password=postgres");

        return new TenantDbContext(optionsBuilder.Options);
    }
}