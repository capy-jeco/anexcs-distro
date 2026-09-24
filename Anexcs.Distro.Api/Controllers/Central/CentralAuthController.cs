using Api.Contracts.Central.Auth;
using Anexcs.Distro.Application.Central.Users.Commands.LoginCentralUser;
using Wolverine;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Central;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/central/auth")]
public class CentralAuthController(IMessageBus messageBus) : Controller
{
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login(
        [FromBody] LoginRequest request,
        CancellationToken cancellationToken)
    {
        var command = new LoginCentralUserCommand(
            request.Email,
            request.Password);

        var token = await messageBus.InvokeAsync<string>(
            command,
            cancellationToken);

        return Ok(new LoginResponse(token));
    }
}