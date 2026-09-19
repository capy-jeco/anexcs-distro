using Anexcs.Distro.Application.Central.TenantDomains.Queries.GetTenantDomainsByTenantId;

using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Central;

[ApiController]
[ApiVersion("1.0")]
[Route("api/{version:apiVersion}/[controller]")]
public class TenantDomainController(ISender sender) : Controller
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByTenantId(
        Guid tenantId,
        CancellationToken cancellationToken)
    {
        var query = new GetTenantDomainsByTenantIdQuery(
            tenantId);

        var domains = await sender.Send(
            query,
            cancellationToken);

        return Ok(domains);
    }
}