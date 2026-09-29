using System.Net;
using System.Net.Http.Json;
using Anexcs.Distro.Application.Central.TenantDomains.Dtos;
using Anexcs.Distro.Domain.Entities.Central;
using Anexcs.Distro.Infrastructure.Persistence.Central;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace Anexcs.Distro.IntegrationTests.Tenants;

public sealed class GetTenantDomainsByTenantIdTests
    : IClassFixture<IntegrationTestFactory>
{
    private readonly IntegrationTestFactory _factory;
    private readonly HttpClient _client;

    public GetTenantDomainsByTenantIdTests(
        IntegrationTestFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetTenantDomains_ShouldReturnDomainsForTenant()
    {
        // Arrange
        var tenantId = Guid.NewGuid();

        await SeedTenantAsync(tenantId);

        await SeedTenantDomainAsync(
            tenantId,
            "acme.com");

        await SeedTenantDomainAsync(
            tenantId,
            "acme.local");

        // Act
        var response = await _client.GetAsync(
            $"/api/v1/Tenants/{tenantId}/domains");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var domains = await response.Content
            .ReadFromJsonAsync<List<TenantDomainResponse>>();

        domains.Should().NotBeNull();
        domains.Should().HaveCount(2);

        domains.Should().Contain(x =>
            x.TenantId == tenantId &&
            x.Domain == "acme.com");

        domains.Should().Contain(x =>
            x.TenantId == tenantId &&
            x.Domain == "acme.local");
    }

    [Fact]
    public async Task GetTenantDomains_ShouldReturnEmptyList_WhenTenantHasNoDomains()
    {
        // Arrange
        var tenantId = Guid.NewGuid();

        await SeedTenantAsync(tenantId);

        // Act
        var response = await _client.GetAsync(
            $"/api/v1/tenants/{tenantId}/domains");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var domains = await response.Content
            .ReadFromJsonAsync<List<TenantDomainResponse>>();

        domains.Should().NotBeNull();
        domains.Should().BeEmpty();
    }

    [Fact]
    public async Task GetTenantDomains_ShouldNotReturnDomainsFromAnotherTenant()
    {
        // Arrange
        var tenantA = Guid.NewGuid();
        var tenantB = Guid.NewGuid();

        await SeedTenantAsync(tenantA);
        await SeedTenantAsync(tenantB);

        await SeedTenantDomainAsync(
            tenantA,
            "acme.com");

        await SeedTenantDomainAsync(
            tenantB,
            "other.com");

        // Act
        var response = await _client.GetAsync(
            $"/api/v1/tenants/{tenantA}/domains");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var domains = await response.Content
            .ReadFromJsonAsync<List<TenantDomainResponse>>();

        domains.Should().NotBeNull();
        domains.Should().HaveCount(1);

        domains![0].TenantId.Should().Be(tenantA);
        domains[0].Domain.Should().Be("acme.com");
    }

    private async Task SeedTenantAsync(Guid tenantId)
    {
        using var scope = _factory.Services.CreateScope();

        var context = scope.ServiceProvider
            .GetRequiredService<CentralDbContext>();

        var tenant = new Tenant(
            tenantId,
            $"Tenant {tenantId}");

        context.Tenants.Add(tenant);

        await context.SaveChangesAsync();
    }

    private async Task SeedTenantDomainAsync(
        Guid tenantId,
        string domain)
    {
        using var scope = _factory.Services.CreateScope();

        var context = scope.ServiceProvider
            .GetRequiredService<CentralDbContext>();

        var tenantDomain = new TenantDomain(
            domain,
            tenantId);

        context.TenantDomains.Add(tenantDomain);

        await context.SaveChangesAsync();
    }
}