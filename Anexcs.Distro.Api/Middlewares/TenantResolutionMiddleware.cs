using Anexcs.Distro.Infrastructure.Persistence.Central;
using Anexcs.Distro.Infrastructure.Tenancy;
using Microsoft.EntityFrameworkCore;

namespace Api.Middlewares;

public class TenantResolutionMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(
        HttpContext context,
        CentralDbContext centralDbContext,
        TenantContext tenantContext)
    {
        var host = context.Request.Host.Host;

        var tenantId = await centralDbContext.TenantDomains
            .Where(d => d.Domain == host)
            .Select(d => (Guid?)d.TenantId)
            .FirstOrDefaultAsync(context.RequestAborted);

        if (tenantId.HasValue)
        {
            tenantContext.SetTenant(tenantId.Value);
        }

        await next(context);
    }
}