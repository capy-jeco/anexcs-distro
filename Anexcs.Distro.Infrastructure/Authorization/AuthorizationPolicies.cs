using Anexcs.Distro.Application.Common.Constants;
using Microsoft.AspNetCore.Authorization;

namespace Anexcs.Distro.Infrastructure.Authorization;

public static class AuthorizationPolicies
{
    public const string RequirePlatformAdministrator = nameof(RequirePlatformAdministrator);
    public const string RequireTenantAdministrator = nameof(RequireTenantAdministrator);
    public const string RequireWarehouseManagement = nameof(RequireWarehouseManagement);
    public const string RequireInventoryManagement = nameof(RequireInventoryManagement);

    public static void Configure(AuthorizationOptions options)
    {
        options.AddPolicy(RequirePlatformAdministrator, policy =>
            policy.RequireRole(CentralRoles.PlatformAdministrator));

        options.AddPolicy(RequireTenantAdministrator, policy =>
            policy.RequireRole(TenantRoles.TenantAdministrator));

        // Multiple roles satisfy this policy — any one of them is sufficient
        options.AddPolicy(RequireWarehouseManagement, policy =>
            policy.RequireRole(
                TenantRoles.TenantAdministrator,
                TenantRoles.WarehouseManager));

        options.AddPolicy(RequireInventoryManagement, policy =>
            policy.RequireRole(
                TenantRoles.TenantAdministrator,
                TenantRoles.WarehouseManager,
                TenantRoles.InventoryManager));
    }
}