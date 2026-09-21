namespace Anexcs.Distro.Domain.Enums;

public sealed class TenantStatus
{
    public static readonly TenantStatus Provisioning =
        new("Provisioning");

    public static readonly TenantStatus Active =
        new("Active");

    public static readonly TenantStatus Suspended =
        new("Suspended");

    public static readonly TenantStatus ProvisioningFailed =
        new("ProvisioningFailed");

    private TenantStatus(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static TenantStatus FromValue(string value)
    {
        return value switch
        {
            "Provisioning" => Provisioning,
            "Active" => Active,
            "Suspended" => Suspended,
            "ProvisioningFailed" => ProvisioningFailed,

            _ => throw new ArgumentOutOfRangeException(
                nameof(value),
                value,
                "Invalid tenant status.")
        };
    }

    public override string ToString() => Value;
}