using Anexcs.Distro.Application.Abstractions.Persistence.Central;
using Anexcs.Distro.Domain.Entities.Central;
using Anexcs.Distro.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Anexcs.Distro.Infrastructure.Persistence.Central.Repositories;

public class CentralUserRepository (
    CentralDbContext context,
    IPasswordHasher<CentralIdentityUser> passwordHasher) : ICentralUserRepository
{
    public async Task<CentralUser?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        var identityUser = await context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

        return identityUser is null ? null : MapToDomain(identityUser);
    }

    public async Task<CentralUser?> FindByEmailAsync(
        string email,
        CancellationToken cancellationToken)
    {
        var normalizedEmail = email.ToUpperInvariant();

        var identityUser = await context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.NormalizedEmail == normalizedEmail, cancellationToken);

        return identityUser is null ? null : MapToDomain(identityUser);
    }

    public async Task<CentralUser?> FindByNameAsync(
        string userName,
        CancellationToken cancellationToken)
    {
        var normalizedUserName = userName.ToUpperInvariant();

        var identityUser = await context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.NormalizedUserName == normalizedUserName, cancellationToken);

        return identityUser is null ? null : MapToDomain(identityUser);
    }

    public async Task<CentralUser?> CheckPasswordAsync(
        CentralUser user,
        string password,
        CancellationToken cancellationToken)
    {
        var identityUser = await context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == user.Id, cancellationToken);

        if (identityUser?.PasswordHash is null)
        {
            return null;
        }

        var verificationResult = passwordHasher.VerifyHashedPassword(
            identityUser, 
            identityUser.PasswordHash, 
            password);

        if (verificationResult == PasswordVerificationResult.Failed)
        {
            return null;
        }

        return user;
    }
    
    private static CentralUser MapToDomain(CentralIdentityUser identityUser)
    {
        return new CentralUser(
            identityUser.Id,
            identityUser.FirstName,
            identityUser.MiddleName,
            identityUser.LastName,
            identityUser.Email!,
            identityUser.PasswordHash ?? string.Empty);
    }
}