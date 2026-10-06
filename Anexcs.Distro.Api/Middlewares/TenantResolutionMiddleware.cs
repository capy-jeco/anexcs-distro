using Anexcs.Distro.Domain.Entities.Central;
using Anexcs.Distro.Domain.Enums;
using Anexcs.Distro.Infrastructure.Persistence.Central;
using Anexcs.Distro.Infrastructure.Tenancy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Api.Middlewares;

public class TenantResolutionMiddleware(
    RequestDelegate next,
    IOptions<TenancyOptions> options)
{
    private readonly HashSet<string> _centralHosts = options.Value.CentralHosts
        .Select(TenantDomain.NormalizeDomain)
        .ToHashSet();

    public async Task InvokeAsync(
        HttpContext context,
        CentralDbContext centralDbContext,
        TenantContext tenantContext,
        TenantConnectionInfo connectionInfo)
    {
        var rawHost = context.Request.Host;

        if (!rawHost.HasValue || Uri.CheckHostName(rawHost.Host) == UriHostNameType.Unknown)
        {
            await RejectAsync(context, StatusCodes.Status400BadRequest,
                "Invalid host", "The request host is missing or malformed.");
            return;
        }

        var host = TenantDomain.NormalizeDomain(rawHost.Host);

        // Central hosts proceed with no tenant; tenant-only endpoints
        // must check ITenantContext.IsResolved themselves.
        if (_centralHosts.Contains(host))
        {
            await next(context);
            return;
        }

        var match = await centralDbContext.TenantDomains
            .AsNoTracking()
            .Where(d => d.Domain == host)
            .Select(d => new
            {
                d.TenantId, 
                d.Tenant.Status,
                d.Tenant.DatabaseName,
                d.Tenant.ServerKey
            })
            .FirstOrDefaultAsync(context.RequestAborted);

        if (match is null)
        {
            await RejectAsync(context, StatusCodes.Status404NotFound,
                "Unknown domain", "No tenant is registered for this domain.");
            return;
        }

        if (match.Status != TenantStatus.Active)
        {
            await RejectAsync(context, StatusCodes.Status403Forbidden,
                "Tenant unavailable", "This tenant is not active.");
            return;
        }
        
        if (match.DatabaseName is null || match.ServerKey is null)
        {
            await RejectAsync(context, StatusCodes.Status500InternalServerError,
                "Tenant misconfigured", "This tenant is active but has no database assigned.");
            return;
        }

        tenantContext.SetTenant(match.TenantId);
        connectionInfo.Set(match.DatabaseName, match.ServerKey);
        
        await next(context);
    }

    private static async Task RejectAsync(
        HttpContext context, int statusCode, string title, string detail)
    {
        context.Response.StatusCode = statusCode;
        await context.Response.WriteAsJsonAsync(new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = detail
        });
    }
}