using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Api.Middlewares;

public class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        switch (exception)
        {
            case ValidationException validationException:
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                await httpContext.Response.WriteAsJsonAsync(new
                {
                    errors = validationException.Errors.Select(e => e.ErrorMessage)
                }, cancellationToken);
                return true;

            case DbUpdateException { InnerException: PostgresException { SqlState: "23505" } pgEx }:
                httpContext.Response.StatusCode = StatusCodes.Status409Conflict;
                await httpContext.Response.WriteAsJsonAsync(new
                {
                    error = "A resource with the same unique value already exists.",
                    constraint = pgEx.ConstraintName
                }, cancellationToken);
                return true;

            default:
                httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                return true;
        }
    }
}