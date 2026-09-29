namespace Anexcs.Distro.Infrastructure.Tenancy;

public class TenancyOptions
{
    public const string SectionName = "Tenancy";

    public string[] CentralHosts { get; set; } = [];
}