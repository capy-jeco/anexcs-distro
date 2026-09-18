using System.Net;
using System.Net.Http.Json;
using System.Text.Json.Nodes;
using Anexcs.Distro.Infrastructure.Persistence.Central;
using Api.Contracts.Tenant;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Anexcs.Distro.IntegrationTests.Tenants;

public class CreateTenantTests(IntegrationTestFactory factory) : IClassFixture<IntegrationTestFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task CreateTenant_ValidRequest_PersistsToDatabase()
    {
        // Arrange
        var request = new
        {
            Data = "{\"tier\":\"enterprise\"}",
            InitialDomain = "testorg.distro.anexcs.com"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/tenants", request);

        // Assert HTTP Response
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var body = await response.Content.ReadFromJsonAsync<CreateTenantResponse>();
        body.Should().NotBeNull();
        body!.Id.Should().NotBeEmpty();

        // Assert Database State
        using var scope = factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<CentralDbContext>();

        var persistedTenant = await dbContext.Tenants.FindAsync(body.Id);
        persistedTenant.Should().NotBeNull();
        
        // jsonb reformats text on the way back out (whitespace/key order),
        // so compare parsed JSON values, not raw strings
        var actualJson = JsonNode.Parse(persistedTenant!.Data);
        var expectedJson = JsonNode.Parse(request.Data);
        JsonNode.DeepEquals(actualJson, expectedJson).Should().BeTrue();
    }

    [Fact]
    public async Task CreateTenant_MissingDomain_ReturnsBadRequest()
    {
        // Arrange
        var request = new
        {
            Data = "{\"tier\":\"enterprise\"}",
            InitialDomain = "" // Invalid
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/tenants", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateTenant_DuplicateDomain_ReturnsConflictOr500()
    {
        // Arrange
        var request = new
        {
            Data = "{}",
            InitialDomain = "duplicate.distro.anexcs.com"
        };

        // Act: Create first tenant
        await _client.PostAsJsonAsync("/api/tenants", request);
        
        // Act: Attempt to create second tenant with same domain
        var response = await _client.PostAsJsonAsync("/api/tenants", request);

        // Assert
        // Depending on how you handle EF Core DbUpdateExceptions globally, 
        // this might be a 500 or a 409 Conflict.
        response.IsSuccessStatusCode.Should().BeFalse(); 
    }
}