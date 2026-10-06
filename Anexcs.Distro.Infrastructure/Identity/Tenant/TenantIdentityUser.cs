using Microsoft.AspNetCore.Identity;

namespace Anexcs.Distro.Infrastructure.Identity;

public class TenantIdentityUser : IdentityUser<Guid>
{
    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public bool IsActive { get; set; } = true;
}