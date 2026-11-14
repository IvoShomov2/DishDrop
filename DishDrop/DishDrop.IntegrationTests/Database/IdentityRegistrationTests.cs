using System.Net;
using System.Text.RegularExpressions;
using DishDrop.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DishDrop.IntegrationTests.Database;

[Trait("Category", "Database")]
public class IdentityRegistrationTests
{
    [Fact]
    public async Task StandardRegistration_PersistsNamesAndStillRequiresConfirmation()
    {
        await using var database = await SqlTestDatabase.CreateAsync();
        await using var factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
            builder.ConfigureServices(services => services.AddDbContext<ApplicationDbContext>(o => o.UseSqlServer(database.ConnectionString))));
        using var client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            BaseAddress = new Uri("https://localhost"), AllowAutoRedirect = false
        });
        var html = await client.GetStringAsync("/Identity/Account/Register");
        var token = WebUtility.HtmlDecode(Regex.Match(html, "name=\"__RequestVerificationToken\"[^>]*value=\"([^\"]+)\"").Groups[1].Value);
        Assert.NotEmpty(token);
        using var response = await client.PostAsync("/Identity/Account/Register", new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["Input.FirstName"] = "Test", ["Input.LastName"] = "Customer", ["Input.Email"] = "registration@example.com",
            ["Input.Password"] = Guid.NewGuid().ToString("N") + "aA1!",
            ["__RequestVerificationToken"] = token
        }.WithConfirmation()));
        Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
        Assert.Contains("RegisterConfirmation", response.Headers.Location!.ToString());
        await using var db = database.Context();
        var user = await db.Users.SingleAsync();
        Assert.Equal("Test", user.FirstName);
        Assert.Equal("Customer", user.LastName);
        Assert.False(user.EmailConfirmed);
        Assert.NotEmpty(user.PasswordHash!);
    }
}

internal static class RegistrationForm
{
    public static Dictionary<string, string> WithConfirmation(this Dictionary<string, string> form)
    {
        form["Input.ConfirmPassword"] = form["Input.Password"];
        return form;
    }
}
