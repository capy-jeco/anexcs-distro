using Anexcs.Distro.Domain.Entities.Central;
using Anexcs.Distro.Domain.Entities.Tenants;

namespace Anexcs.Distro.Application.Common.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateCentralUserToken(CentralUser user);
    string GenerateTenantUserToken(TenantUser user, string tenantId);
}