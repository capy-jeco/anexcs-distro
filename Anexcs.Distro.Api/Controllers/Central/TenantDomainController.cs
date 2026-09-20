using Anexcs.Distro.Application.Central.TenantDomains.Queries.GetTenantDomainsByTenantId;
using Anexcs.Distro.Application.Central.TenantDomains.Dtos;

using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Wolverine;

namespace Api.Controllers.Central;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/tenants/{tenantId:guid}/domains")]
public class TenantDomainController(IMessageBus messageBus) : Controller
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

        var domains = await messageBus.InvokeAsync<IReadOnlyList<TenantDomainResponse>>(
            query,
            cancellationToken);

        return Ok(domains);
    }
}