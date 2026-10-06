using Anexcs.Distro.Application.Abstractions.Persistence.Central;
using Anexcs.Distro.Infrastructure.Identity.Central;
using Microsoft.EntityFrameworkCore;

namespace Anexcs.Distro.Infrastructure.Persistence.Central.Repositories;

public class CentralUserRepository(CentralDbContext context) : ICentralUserRepository
{
    public async Task<Domain.Entities.Central.CentralUser?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        var identityUser = await context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

        return identityUser is null ? null : MapToDomain(identityUser);
    }

    public async Task<Domain.Entities.Central.CentralUser?> FindByEmailAsync(
        string email,
        CancellationToken cancellationToken)
    {
        var normalizedEmail = email.ToUpperInvariant();

        var identityUser = await context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.NormalizedEmail == normalizedEmail, cancellationToken);

        return identityUser is null ? null : MapToDomain(identityUser);
    }

    public async Task<Domain.Entities.Central.CentralUser?> FindByNameAsync(
        string userName,
        CancellationToken cancellationToken)
    {
        var normalizedUserName = userName.ToUpperInvariant();

        var identityUser = await context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.NormalizedUserName == normalizedUserName, cancellationToken);

        return identityUser is null ? null : MapToDomain(identityUser);
    }

    private static Domain.Entities.Central.CentralUser MapToDomain(CentralIdentityUser identityUser)
    {
        return new Domain.Entities.Central.CentralUser(
            identityUser.Id,
            identityUser.FirstName,
            identityUser.MiddleName,
            identityUser.LastName,
            identityUser.Email!);
    }
}