namespace Anexcs.Distro.Domain.Entities.Central;

public class CentralUser
{
    public Guid Id { get; private set; }
    public string FirstName { get; private set; } = null!;
    public string? MiddleName { get; private set; }
    public string LastName { get; private set; } = null!;
    public string Email { get; private set; } = null!;
    public string PasswordHash { get; private set; } = null!;
    public bool IsActive { get; private set; } = true;
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime? UpdatedAtUtc { get; private set; }
    public DateTime? EmailVerifiedAtUtc { get; private set; }

    // Private constructor for domain mapping and ORM deserialization
    private CentralUser() { }

    public CentralUser(
        Guid id,
        string firstName,
        string? middleName,
        string lastName,
        string email,
        string passwordHash,
        bool isActive = true,
        DateTime? createdAtUtc = null,
        DateTime? updatedAtUtc = null,
        DateTime? emailVerifiedAtUtc = null)
    {
        Id = id;
        FirstName = firstName;
        MiddleName = middleName;
        LastName = lastName;
        Email = email;
        PasswordHash = passwordHash;
        IsActive = isActive;
        CreatedAtUtc = createdAtUtc ?? DateTime.UtcNow;
        UpdatedAtUtc = updatedAtUtc;
        EmailVerifiedAtUtc = emailVerifiedAtUtc;
    }

    // Static Factory Method for creating fresh instances
    public static CentralUser Create(
        string firstName,
        string? middleName,
        string lastName,
        string email,
        string passwordHash)
    {
        return new CentralUser(
            Guid.NewGuid(),
            firstName,
            middleName,
            lastName,
            email,
            passwordHash);
    }

    // --- Domain Behaviors ---

    public void UpdateProfile(string firstName, string? middleName, string lastName)
    {
        FirstName = firstName;
        MiddleName = middleName;
        LastName = lastName;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public void UpdatePassword(string passwordHash)
    {
        PasswordHash = passwordHash;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public void VerifyEmail()
    {
        EmailVerifiedAtUtc = DateTime.UtcNow;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public void Activate()
    {
        IsActive = true;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        IsActive = false;
        UpdatedAtUtc = DateTime.UtcNow;
    }
}