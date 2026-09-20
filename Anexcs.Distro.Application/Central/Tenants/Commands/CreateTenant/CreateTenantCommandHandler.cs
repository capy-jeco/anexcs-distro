using Anexcs.Distro.Application.Abstractions.Persistence.Central;
using Anexcs.Distro.Domain.Entities.Central;

namespace Anexcs.Distro.Application.Central.Tenants.Commands.CreateTenant;

public sealed class CreateTenantCommandHandler(
    ITenantRepository tenantRepository,
    ICentralUnitOfWork unitOfWork)
{
    public async Task<Guid> Handle(
        CreateTenantCommand request,
        CancellationToken cancellationToken)
    {
        var tenantId = Guid.NewGuid();

        var tenant = new Tenant(
            tenantId,
            request.Data);

        var tenantDomain = new TenantDomain(
            request.InitialDomain,
            tenantId);

        tenant.Domains.Add(tenantDomain);

        await tenantRepository.AddAsync(
            tenant,
            cancellationToken);

        await unitOfWork.SaveChangesAsync(
            cancellationToken);

        return tenant.Id;
    }
}