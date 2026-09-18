using MediatR;

namespace Anexcs.Distro.Application.Tenants.Commands.CreateTenant;

public record CreateTenantCommand(string Data, string InitialDomain) : IRequest<Guid>;