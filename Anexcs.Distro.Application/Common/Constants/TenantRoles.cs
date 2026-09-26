namespace Anexcs.Distro.Application.Common.Constants;

public static class TenantRoles
{
    public const string TenantAdministrator = "TenantAdministrator";
    public const string WarehouseManager = "WarehouseManager";
    public const string WarehouseOperator = "WarehouseOperator";
    public const string InventoryManager = "InventoryManager";

    public static readonly string[] All =
    [
        TenantAdministrator,
        WarehouseManager,
        WarehouseOperator,
        InventoryManager
    ];
}