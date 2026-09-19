
namespace Anexcs.Distro.Application.Abstractions.Persistence.Central;

public interface ITenantRepository
{
    Task AddAsync(
        Domain.Entities.Central.Tenant tenant,
        CancellationToken cancellationToken);
    
    Task<Domain.Entities.Central.Tenant?> GetByDomainAsync(
        string domain,
        CancellationToken cancellationToken);
} 