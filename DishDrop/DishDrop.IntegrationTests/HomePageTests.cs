using DishDrop.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Xunit;

namespace DishDrop.IntegrationTests;

[Trait("Category", "NoDatabase")]
public class HomePageTests
{
    [Fact]
    public async Task HomePage_ReturnsSuccessAndTemplateHeading_WithoutDatabaseAccess()
    {
        await using var factory = new TestApplicationFactory();
        using var client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            BaseAddress = new Uri("https://localhost"),
            AllowAutoRedirect = false
        });

        using var response = await client.GetAsync("/");

        response.EnsureSuccessStatusCode();
        var html = await response.Content.ReadAsStringAsync();
        Assert.Contains("<h1 class=\"display-4\">Welcome</h1>", html);
    }

    [Fact]
    public async Task TestHost_UsesOwnConfigurationWithoutSecretsAndBlocksDatabaseConnections()
    {
        await using var factory = new TestApplicationFactory();
        using var scope = factory.Services.CreateScope();
        var services = scope.ServiceProvider;
        Assert.Equal("Testing", services.GetRequiredService<IHostEnvironment>().EnvironmentName);

        var configuration = services.GetRequiredService<IConfiguration>();
        var root = Assert.IsAssignableFrom<IConfigurationRoot>(configuration);
        Assert.DoesNotContain(root.Providers.OfType<JsonConfigurationProvider>(),
            provider => Path.GetFileName(provider.Source.Path) == "secrets.json");
        Assert.Equal("DishDropHttpTests",
            new SqlConnectionStringBuilder(configuration.GetConnectionString("DefaultConnection")).InitialCatalog);

        var db = services.GetRequiredService<ApplicationDbContext>();
        var synchronous = Assert.Throws<InvalidOperationException>(() => db.Database.OpenConnection());
        Assert.Contains("must not open a database connection", synchronous.Message);
        var asynchronous = await Assert.ThrowsAsync<InvalidOperationException>(() => db.Database.OpenConnectionAsync());
        Assert.Contains("must not open a database connection", asynchronous.Message);
    }
}
