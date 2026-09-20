using Anexcs.Distro.Application.Common.Health;
using Microsoft.AspNetCore.Mvc;
using Wolverine;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    private readonly IMessageBus _messageBus;

    public HealthController(IMessageBus messageBus)
    {
        _messageBus = messageBus;
    }

    [HttpGet]
    public async Task<IActionResult> Get(
        CancellationToken cancellationToken)
    {
        var result = await _messageBus.InvokeAsync<HealthResponse>(
            new GetHealthQuery(),
            cancellationToken);

        return Ok(result);
    }
}