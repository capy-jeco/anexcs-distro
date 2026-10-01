using Anexcs.Distro.Application.Abstractions.Tenancy;
using Microsoft.AspNetCore.Mvc;

namespace Api.Middlewares;

public class TenantMembershipGuardMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context, ITenantContext tenantContext)
    {
        var isAuthenticated = context.User.Identity?.IsAuthenticated == true;

        if (!isAuthenticated)
        {
            await next(context);
            return;
        }

        var userType = context.User.FindFirst("user_type")?.Value;
        var tenantClaim = context.User.FindFirst("tenant_id")?.Value;

        switch (userType)
        {
            case "central":
                if (tenantContext.IsResolved)
                {
                    await RejectAsync(context, "Central token on tenant route",
                        "Central user tokens cannot be used against a tenant-resolved domain.");
                    return;
                }
                break;

            case "tenant":
                if (!tenantContext.IsResolved)
                {
                    await RejectAsync(context, "Tenant token on central route",
                        "Tenant user tokens cannot be used against a central domain.");
                    return;
                }

                if (!Guid.TryParse(tenantClaim, out var tokenTenantId))
                {
                    await RejectAsync(context, "Malformed tenant claim",
                        "The token's tenant identifier could not be parsed.");
                    return;
                }

                if (tokenTenantId != tenantContext.TenantId)
                {
                    await RejectAsync(context, "Tenant mismatch",
                        "This token was not issued for the resolved tenant.");
                    return;
                }
                break;

            default:
                await RejectAsync(context, "Unrecognized token type",
                    "The token's user_type claim is missing or unrecognized.");
                return;
        }

        await next(context);
    }

    private static async Task RejectAsync(HttpContext context, string title, string detail)
    {
        context.Response.StatusCode = StatusCodes.Status403Forbidden;
        await context.Response.WriteAsJsonAsync(new ProblemDetails
        {
            Status = StatusCodes.Status403Forbidden,
            Title = title,
            Detail = detail
        });
    }
}