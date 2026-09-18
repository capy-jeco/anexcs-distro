using Anexcs.Distro.Application.Tenants.Commands.CreateTenant;
using Api.Contracts.Central.Tenant;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Central;

[ApiController]
[Route("api/[controller]")]
public class TenantsController(ISender sender) : ControllerBase
{

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create(
        [FromBody] CreateTenantCommand command,
        CancellationToken cancellationToken)
    {
        var tenantId = await sender.Send(command, cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { id = tenantId },
            new CreateTenantResponse(tenantId));
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        // We'll implement this later.
        return NotFound();
    }
}