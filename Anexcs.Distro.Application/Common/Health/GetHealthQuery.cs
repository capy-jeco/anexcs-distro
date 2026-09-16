using MediatR;

namespace Anexcs.Distro.Application.Common.Health;

public sealed record GetHealthQuery : IRequest<HealthResponse>;

public sealed record HealthResponse(
    string Status,
    DateTime Timestamp);