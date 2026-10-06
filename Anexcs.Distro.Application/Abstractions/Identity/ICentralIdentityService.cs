using Anexcs.Distro.Domain.Entities.Central;

namespace Anexcs.Distro.Application.Abstractions.Identity;

public interface ICentralIdentityService
{
    Task<CentralAuthenticationResult> ValidateCredentialsAsync(
        string email, string password, CancellationToken cancellationToken);
}

public sealed record CentralAuthenticationResult(
    bool Succeeded,
    CentralUser? User,
    IReadOnlyList<string> Roles);