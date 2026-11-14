using DishDrop.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DishDrop.IntegrationTests;

[Trait("Category", "NoDatabase")]
public class HomePageTests
{
    [Fact]
    public async Task HomePage_ReturnsSuccessAndTemplateHeading_WithoutDatabaseAccess()
    {
        await using var factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.UseEnvironment("Testing");
                builder.ConfigureServices(services =>
                    services.AddDbContext<ApplicationDbContext>(options =>
                        options.AddInterceptors(new RejectDatabaseConnections())));
            });
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

    private sealed class RejectDatabaseConnections : DbConnectionInterceptor
    {
        public override InterceptionResult ConnectionOpening(
            System.Data.Common.DbConnection connection,
            ConnectionEventData eventData,
            InterceptionResult result) =>
            throw new InvalidOperationException("The smoke test must not open a database connection.");

        public override ValueTask<InterceptionResult> ConnectionOpeningAsync(
            System.Data.Common.DbConnection connection,
            ConnectionEventData eventData,
            InterceptionResult result,
            CancellationToken cancellationToken = default) =>
            throw new InvalidOperationException("The smoke test must not open a database connection.");
    }
}
