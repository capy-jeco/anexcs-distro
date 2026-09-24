using Anexcs.Distro.Application.Abstractions.Persistence.Central;
using Anexcs.Distro.Application.Common.Interfaces;
using Anexcs.Distro.Domain.Entities.Central;

namespace Anexcs.Distro.Application.Central.Users.Commands.LoginCentralUser;

public sealed class LoginCentralUserCommandHandler(
    ICentralUserRepository userRepository,
    IJwtTokenGenerator jwtTokenGenerator)
{
    public async Task<string> Handle(
        LoginCentralUserCommand command,
        CancellationToken cancellationToken)
    {
        var user = await userRepository.FindByEmailAsync(command.Email, cancellationToken);

        if (user is null || !user.IsActive)
            throw new UnauthorizedAccessException("Invalid credentials.");

        var authenticatedUser = await userRepository.CheckPasswordAsync(user, command.Password, cancellationToken);
        if (authenticatedUser is null)
            throw new UnauthorizedAccessException("Invalid credentials.");

        return jwtTokenGenerator.GenerateCentralUserToken(authenticatedUser);
    }
}