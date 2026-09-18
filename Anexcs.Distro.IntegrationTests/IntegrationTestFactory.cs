using Anexcs.Distro.Infrastructure.Persistence.Central;
using Api;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.PostgreSql;

namespace Anexcs.Distro.IntegrationTests;

// ReSharper disable once ClassNeverInstantiated.Global
public class IntegrationTestFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder("postgres:17-alpine")
        .WithDatabase("anexcs_distro_test")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            // Remove the existing CentralDbContext configuration
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<CentralDbContext>));
            if (descriptor != null) services.Remove(descriptor);

            // Add it back using the Test container connection string
            services.AddDbContext<CentralDbContext>(options =>
                options.UseNpgsql(_dbContainer.GetConnectionString()));
        });
    }

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
        
        // Ensure the database is created and migrations are applied
        using var scope = Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<CentralDbContext>();
        await context.Database.MigrateAsync(); 
    }

    public new async Task DisposeAsync()
    {
        await _dbContainer.StopAsync();
    }
}