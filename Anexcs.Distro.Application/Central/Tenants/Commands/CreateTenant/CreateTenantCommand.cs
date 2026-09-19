using MediatR;

namespace Anexcs.Distro.Application.Central.Tenants.Commands.CreateTenant;

public record CreateTenantCommand(string Data, string InitialDomain) : IRequest<Guid>;