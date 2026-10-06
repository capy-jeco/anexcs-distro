using Anexcs.Distro.Application.Abstractions.Identity;
using Anexcs.Distro.Application.Abstractions.Persistence.Central;
using Anexcs.Distro.Application.Common.Interfaces;
using Anexcs.Distro.Domain.Entities.Central;

namespace Anexcs.Distro.Application.Central.Users.Commands.LoginCentralUser;

public sealed class LoginCentralUserCommandHandler(
    ICentralIdentityService identityService,
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

        var authenticatedUser = await identityService.ValidateCredentialsAsync(
            user.Email, 
            command.Password, 
            cancellationToken);
        
        if (authenticatedUser is null)
            throw new UnauthorizedAccessException("Invalid credentials.");
        
        var roles = authenticatedUser.Roles.ToList();
        
        return jwtTokenGenerator.GenerateCentralUserToken(authenticatedUser.User!, roles);
    }
}