using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace DishDrop.IntegrationTests;

[Trait("Category", "NoDatabase")]
public class IdentityPageTests
{
    [Theory]
    [InlineData("/Identity/Account/Login", "Login")]
    [InlineData("/Identity/Account/Register", "Input.FirstName")]
    public async Task IdentityPages_OpenWithApplicationUser(string path, string expected)
    {
        await using var factory = new WebApplicationFactory<Program>();
        using var client = factory.CreateClient(new WebApplicationFactoryClientOptions { BaseAddress = new Uri("https://localhost") });
        using var response = await client.GetAsync(path);
        response.EnsureSuccessStatusCode();
        Assert.Contains(expected, await response.Content.ReadAsStringAsync());
    }

    [Fact]
    public async Task Manage_RequiresAuthentication()
    {
        await using var factory = new WebApplicationFactory<Program>();
        using var client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            BaseAddress = new Uri("https://localhost"), AllowAutoRedirect = false
        });
        using var response = await client.GetAsync("/Identity/Account/Manage");
        Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
        Assert.Contains("/Identity/Account/Login", response.Headers.Location!.ToString());
    }
}
