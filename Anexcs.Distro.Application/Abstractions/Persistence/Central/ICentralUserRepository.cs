namespace Anexcs.Distro.Application.Abstractions.Persistence.Central;

public interface ICentralUserRepository
{
    Task<Domain.Entities.Central.CentralUser?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken);
    
    Task<Domain.Entities.Central.CentralUser?> FindByEmailAsync(
        string email,
        CancellationToken cancellationToken);
    
    Task<Domain.Entities.Central.CentralUser?> FindByNameAsync(
        string userName,
        CancellationToken cancellationToken);
    
    Task<Domain.Entities.Central.CentralUser?> CheckPasswordAsync(
        Domain.Entities.Central.CentralUser user,
        string password,
        CancellationToken cancellationToken);
    
    Task<IList<string>> GetRolesAsync(
        Domain.Entities.Central.CentralUser user,
        CancellationToken cancellationToken);
}