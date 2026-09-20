
namespace Anexcs.Distro.Application.Common.Health;

public sealed record GetHealthQuery;

public sealed record HealthResponse(
    string Status,
    DateTime Timestamp);