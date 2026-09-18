using Anexcs.Distro.Application.Abstractions.Persistence;
using Anexcs.Distro.Domain.Entities.Central;
using MediatR;

namespace Anexcs.Distro.Application.Tenants.Commands.CreateTenant;

public sealed class CreateTenantCommandHandler(
    ITenantRepository tenantRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<CreateTenantCommand, Guid>
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