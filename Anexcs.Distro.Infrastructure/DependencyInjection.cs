using Anexcs.Distro.Infrastructure.Persistence.Central;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

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
        
        return services;
    }
}