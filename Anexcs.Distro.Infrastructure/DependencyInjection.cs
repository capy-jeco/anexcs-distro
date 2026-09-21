// Distro.Application
using Anexcs.Distro.Application.Abstractions.Persistence.Central;
using Anexcs.Distro.Application.Abstractions.Persistence.Tenant;
using Anexcs.Distro.Application.Abstractions.Tenancy;

// Distro.Infrastructure
using Anexcs.Distro.Infrastructure.Persistence.Central;
using Anexcs.Distro.Infrastructure.Persistence.Central.Repositories;
using Anexcs.Distro.Infrastructure.Persistence.Tenant;
using Anexcs.Distro.Infrastructure.Tenancy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Anexcs.Distro.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<CentralDbContext>(options =>
        {
            options.UseNpgsql(
                configuration.GetConnectionString("CentralDatabase"));
        });
        
        services.AddDbContext<TenantDbContext>(options =>
        {
            options.UseNpgsql(
                configuration.GetConnectionString("TenantDatabase"),
                npgsql =>
                {
                    npgsql.MigrationsAssembly(
                        typeof(TenantDbContext).Assembly.FullName);
                });
        });
        
        services.AddScoped<
            ITenantDatabaseProvisioner,
            TenantDatabaseProvisioner>();
        
        services.AddScoped<ICentralUnitOfWork>(sp =>
            sp.GetRequiredService<CentralDbContext>());
        services.AddScoped<ITenantUnitOfWork>(sp =>
            sp.GetRequiredService<TenantDbContext>());
        
        services.AddScoped<ITenantRepository, TenantRepository>();
        services.AddScoped<ITenantDomainRepository, TenantDomainRepository>();
        
        return services;
    }
}