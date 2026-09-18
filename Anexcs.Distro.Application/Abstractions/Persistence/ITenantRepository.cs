using Anexcs.Distro.Domain.Entities.Central;

namespace Anexcs.Distro.Application.Abstractions.Persistence;

public interface ITenantRepository
{
    Task AddAsync(
        Tenant tenant,
        CancellationToken cancellationToken);
}