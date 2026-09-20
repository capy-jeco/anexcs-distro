
namespace Anexcs.Distro.Application.Common.Health;

public sealed class GetHealthQueryHandler
{
    public Task<HealthResponse> Handle(
        GetHealthQuery request,
        CancellationToken cancellationToken)
    {
        var response = new HealthResponse(
            "ok",
            DateTime.UtcNow);

        return Task.FromResult(response);
    }
}