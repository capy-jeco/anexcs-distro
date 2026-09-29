using Anexcs.Distro.Application.Abstractions.Tenancy;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace Anexcs.Distro.IntegrationTests.Tenancy;

[ApiController]
[ApiVersionNeutral]
[Route("__test/tenant-context")]
public class TenantContextProbeController(ITenantContext tenantContext) : ControllerBase
{
    [HttpGet]
    public IActionResult Get() => Ok(new
    {
        tenantContext.IsResolved,
        TenantId = tenantContext.IsResolved ? tenantContext.TenantId : (Guid?)null
    });
}