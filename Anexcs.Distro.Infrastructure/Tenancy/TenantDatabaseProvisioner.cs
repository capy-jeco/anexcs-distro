using Anexcs.Distro.Application.Abstractions.Tenancy;
using Anexcs.Distro.Application.Common.Constants;
using Anexcs.Distro.Infrastructure.Persistence.Tenant;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using Npgsql;

namespace Anexcs.Distro.Infrastructure.Tenancy;

public sealed class TenantDatabaseProvisioner(IConfiguration configuration)
    : ITenantDatabaseProvisioner
{
    private const string DefaultServerKey = "primary";

    public async Task<TenantDatabaseProvisionResult> ProvisionAsync(
        Guid tenantId,
        CancellationToken cancellationToken)
    {
        var serverKey = DefaultServerKey;
        var databaseName = $"anexcs_tenant_{tenantId:N}";

        var adminConnectionString = configuration[
            $"TenantDatabase:Servers:{serverKey}:AdminConnectionString"];

        if (string.IsNullOrWhiteSpace(adminConnectionString))
        {
            throw new InvalidOperationException(
                $"No admin connection string configured for server '{serverKey}'.");
        }

        await CreateDatabaseAsync(
            adminConnectionString, 
            databaseName, 
            cancellationToken);
        
        await MigrateDatabaseAsync(
            adminConnectionString, 
            databaseName, 
            cancellationToken);

        return new TenantDatabaseProvisionResult(databaseName, serverKey);
    }

    private static async Task CreateDatabaseAsync(
        string adminConnectionString,
        string databaseName,
        CancellationToken cancellationToken)
    {
        await using var connection = new NpgsqlConnection(adminConnectionString);
        await connection.OpenAsync(cancellationToken);

        await using var command = connection.CreateCommand();
        command.CommandText = $"""CREATE DATABASE "{databaseName}" """;

        try
        {
            await command.ExecuteNonQueryAsync(cancellationToken);
        }
        catch (PostgresException ex) when (ex.SqlState == "42P04")
        {
            throw new InvalidOperationException(
                $"Tenant database '{databaseName}' already exists.", ex);
        }
    }

    private static async Task MigrateDatabaseAsync(
        string adminConnectionString,
        string databaseName,
        CancellationToken cancellationToken)
    {
        var builder = new NpgsqlConnectionStringBuilder(
            adminConnectionString)
        {
            Database = databaseName
        };

        var options = new DbContextOptionsBuilder<TenantDbContext>()
            .UseNpgsql(
                builder.ConnectionString,
                npgsql =>
                {
                    npgsql.MigrationsAssembly(
                        typeof(TenantDbContext).Assembly.FullName);
                })
            .Options;

        await using var context = new TenantDbContext(options);
        
        await context.Database.MigrateAsync(cancellationToken);
        
        var roleStore = new RoleStore<IdentityRole>(context);
        var roleManager = new RoleManager<IdentityRole>(
            roleStore,
            roleValidators: [],
            keyNormalizer: new UpperInvariantLookupNormalizer(),
            errors: new IdentityErrorDescriber(),
            logger: NullLogger<RoleManager<IdentityRole>>.Instance);

        foreach (var role in TenantRoles.All)
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
}