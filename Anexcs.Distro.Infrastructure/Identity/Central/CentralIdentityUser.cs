using Microsoft.AspNetCore.Identity;

namespace Anexcs.Distro.Infrastructure.Identity.Central;

public class CentralIdentityUser : IdentityUser<Guid>
{
    public string FirstName { get; set; } = null!;
    public string? MiddleName { get; set; }
    public string LastName { get; set; } = null!;
    public bool IsActive { get; set; } = true;
}