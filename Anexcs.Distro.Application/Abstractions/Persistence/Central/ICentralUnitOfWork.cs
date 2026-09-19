namespace Anexcs.Distro.Application.Abstractions.Persistence.Central;

public interface ICentralUnitOfWork
{
    Task<int> SaveChangesAsync(
        CancellationToken cancellationToken);
}