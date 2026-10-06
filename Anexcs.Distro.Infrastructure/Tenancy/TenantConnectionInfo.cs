namespace Anexcs.Distro.Infrastructure.Tenancy;

public class TenantConnectionInfo
{
    public bool IsSet { get; private set; }
    public string DatabaseName { get; private set; } = null!;
    public string ServerKey { get; private set; } = null!;

    public void Set(string databaseName, string serverKey)
    {
        if (IsSet)
            throw new InvalidOperationException("Tenant connection info already set for this request.");

        DatabaseName = databaseName;
        ServerKey = serverKey;
        IsSet = true;
    }
}