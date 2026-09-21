namespace Anexcs.Distro.Application.Abstractions.Tenancy;

public sealed record TenantDatabaseProvisionResult(string DatabaseName, string ServerKey);