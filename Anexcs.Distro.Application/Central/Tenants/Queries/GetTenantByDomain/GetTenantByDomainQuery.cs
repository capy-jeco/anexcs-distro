using MediatR;

using Anexcs.Distro.Application.Central.Tenants.Dtos;

namespace Anexcs.Distro.Application.Central.Tenants.Queries.GetTenantByDomain;

public class GetTenantByDomainQuery : IRequest<TenantResponse>
{
    /// <summary>
    /// The domain name to search for (e.g., "client.anexcs.com")
    /// </summary>
    public string Domain { get; set; } = string.Empty;
}