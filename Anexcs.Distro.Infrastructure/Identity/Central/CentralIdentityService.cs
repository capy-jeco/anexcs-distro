using Anexcs.Distro.Application.Abstractions.Identity;
using Anexcs.Distro.Application.Abstractions.Persistence.Central;
using Anexcs.Distro.Infrastructure.Persistence.Central;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Anexcs.Distro.Infrastructure.Identity.Central;

public class CentralIdentityService(
    CentralDbContext context,
    ICentralUserRepository userRepository,
    IPasswordHasher<CentralIdentityUser> passwordHasher)
    : ICentralIdentityService
{
    public async Task<CentralAuthenticationResult> ValidateCredentialsAsync(
        string email, string password, CancellationToken cancellationToken)
    {
        var domainUser = await userRepository.FindByEmailAsync(email, cancellationToken);
        if (domainUser is null || !domainUser.IsActive)
            return new CentralAuthenticationResult(false, null, []);

        var identityUser = await context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == domainUser.Id, cancellationToken);

        if (identityUser?.PasswordHash is null)
            return new CentralAuthenticationResult(false, null, []);

        var verification = passwordHasher.VerifyHashedPassword(
            identityUser, identityUser.PasswordHash, password);

        if (verification == PasswordVerificationResult.Failed)
            return new CentralAuthenticationResult(false, null, []);

        var roles = await context.UserRoles
            .AsNoTracking()
            .Where(ur => ur.UserId == identityUser.Id)
            .Join(context.Roles, ur => ur.RoleId, r => r.Id, (_, r) => r.Name!)
            .ToListAsync(cancellationToken);

        return new CentralAuthenticationResult(true, domainUser, roles);
    }
}