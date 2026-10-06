using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Anexcs.Distro.Infrastructure.Tenancy;

public class TenantConnectionStringResolver(
    TenantConnectionInfo connectionInfo,
    IConfiguration configuration)
{
    public string Resolve()
    {
        if (!connectionInfo.IsSet)
        {
            throw new InvalidOperationException(
                "No tenant database has been resolved for the current request.");
        }

        var baseConnectionString = configuration[
            $"TenantDatabase:Servers:{connectionInfo.ServerKey}:AppConnectionString"];

        if (string.IsNullOrWhiteSpace(baseConnectionString))
        {
            throw new InvalidOperationException(
                $"No connection string configured for server '{connectionInfo.ServerKey}'.");
        }

        var builder = new NpgsqlConnectionStringBuilder(baseConnectionString)
        {
            Database = connectionInfo.DatabaseName
        };

        return builder.ConnectionString;
    }
}