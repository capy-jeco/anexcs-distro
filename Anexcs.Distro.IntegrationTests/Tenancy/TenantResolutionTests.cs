using System.Net;
using System.Net.Http.Json;
using Anexcs.Distro.Domain.Entities.Central;
using Anexcs.Distro.Infrastructure.Persistence.Central;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Anexcs.Distro.IntegrationTests.Tenancy;

public sealed class TenantResolutionTests(IntegrationTestFactory factory)
    : IClassFixture<IntegrationTestFactory>
{
    private const string ProbeRoute = "/__test/tenant-context";
    private readonly HttpClient _client = factory.CreateClient();

    private sealed record ProbeResponse(bool IsResolved, Guid? TenantId);

    [Fact]
    public async Task ValidTenantDomain_ResolvesTenant()
    {
        var tenantId = await SeedTenantAsync("valid.resolution.test");

        var response = await GetAsHostAsync("valid.resolution.test");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<ProbeResponse>();
        body!.IsResolved.Should().BeTrue();
        body.TenantId.Should().Be(tenantId);
    }

    [Fact]
    public async Task DomainWithDifferentCase_ResolvesSameTenant()
    {
        var tenantId = await SeedTenantAsync("casing.resolution.test");

        var response = await GetAsHostAsync("CaSiNg.Resolution.TEST");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<ProbeResponse>();
        body!.TenantId.Should().Be(tenantId);
    }

    [Fact]
    public async Task UnknownDomain_ReturnsNotFound()
    {
        var response = await GetAsHostAsync("nobody.resolution.test");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task InactiveTenantDomain_ReturnsForbidden()
    {
        await SeedTenantAsync("suspended.resolution.test", activate: false);

        var response = await GetAsHostAsync("suspended.resolution.test");

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task CentralHost_PassesThroughWithoutTenant()
    {
        // TestServer's default host is "localhost", listed in CentralHosts
        var response = await _client.GetAsync(ProbeRoute);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<ProbeResponse>();
        body!.IsResolved.Should().BeFalse();
        body.TenantId.Should().BeNull();
    }

    [Fact]
    public async Task ConsecutiveRequests_DifferentTenants_DoNotShareContext()
    {
        var tenantA = await SeedTenantAsync("a.isolation.test");
        var tenantB = await SeedTenantAsync("b.isolation.test");

        var a = await (await GetAsHostAsync("a.isolation.test"))
            .Content.ReadFromJsonAsync<ProbeResponse>();
        var b = await (await GetAsHostAsync("b.isolation.test"))
            .Content.ReadFromJsonAsync<ProbeResponse>();

        a!.TenantId.Should().Be(tenantA);
        b!.TenantId.Should().Be(tenantB);
    }

    [Fact]
    public async Task DuplicateDomain_DifferentCase_IsRejectedByDatabase()
    {
        var tenantId = await SeedTenantAsync("dup.resolution.test");

        using var scope = factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<CentralDbContext>();
        db.TenantDomains.Add(new TenantDomain("DUP.Resolution.TEST", tenantId));

        var act = () => db.SaveChangesAsync();

        await act.Should().ThrowAsync<DbUpdateException>();
    }

    private async Task<HttpResponseMessage> GetAsHostAsync(string host)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, ProbeRoute);
        request.Headers.Host = host;
        return await _client.SendAsync(request);
    }

    private async Task<Guid> SeedTenantAsync(string domain, bool activate = true)
    {
        using var scope = factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<CentralDbContext>();

        var tenant = new Tenant(Guid.NewGuid(), "{}");
        if (activate) tenant.Activate();

        db.Tenants.Add(tenant);
        db.TenantDomains.Add(new TenantDomain(domain, tenant.Id));
        await db.SaveChangesAsync();

        return tenant.Id;
    }
}