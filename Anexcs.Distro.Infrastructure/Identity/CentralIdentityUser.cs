using Microsoft.AspNetCore.Identity;

namespace Anexcs.Distro.Infrastructure.Identity;

public class CentralIdentityUser : IdentityUser<Guid>
{
    public string FirstName { get; set; } = null!;

    public string? MiddleName { get; set; }

    public string LastName { get; set; } = null!;

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAtUtc { get; set; }

    public DateTime? LastLoginAtUtc { get; set; }

    public DateTime? EmailVerifiedAtUtc { get; set; }
}