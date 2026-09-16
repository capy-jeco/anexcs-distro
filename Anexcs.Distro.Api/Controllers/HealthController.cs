using Anexcs.Distro.Application.Common.Health;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    private readonly ISender _sender;

    public HealthController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    public async Task<IActionResult> Get(
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            new GetHealthQuery(),
            cancellationToken);

        return Ok(result);
    }
}