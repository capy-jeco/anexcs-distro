using Anexcs.Distro.Application.Abstractions.Persistence.Central;
using Anexcs.Distro.Application.Abstractions.Tenancy;
using Anexcs.Distro.Domain.Entities.Central;
using Anexcs.Distro.Domain.Enums;

namespace Anexcs.Distro.Application.Central.Tenants.Commands.CreateTenant;

public sealed class CreateTenantCommandHandler(
    ITenantRepository tenantRepository,
    ICentralUnitOfWork unitOfWork,
    ITenantDatabaseProvisioner tenantDatabaseProvisioner)
{
    public async Task<Guid> Handle(
        CreateTenantCommand request,
        CancellationToken cancellationToken)
    {
        var tenantId = Guid.NewGuid();

        var tenant = new Tenant(
            tenantId,
            request.Data);

        tenant.MarkAsProvisioning();

        var tenantDomain = new TenantDomain(
            request.InitialDomain,
            tenantId);

        tenant.Domains.Add(tenantDomain);

        await tenantRepository.AddAsync(
            tenant,
            cancellationToken);

        await unitOfWork.SaveChangesAsync(
            cancellationToken);

        try
        {
            await tenantDatabaseProvisioner.ProvisionAsync(
                tenantId,
                cancellationToken);

            tenant.Activate();

            await unitOfWork.SaveChangesAsync(
                cancellationToken);
        }
        catch
        {
            tenant.MarkProvisioningFailed();

            await unitOfWork.SaveChangesAsync(
                cancellationToken);

            throw;
        }

        return tenant.Id;
    }
}