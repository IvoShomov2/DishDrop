using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DishDrop.IntegrationTests.Database;

[Trait("Category", "Database")]
public class SeedCommandTests
{
    [Fact]
    public async Task ExplicitSeedCommands_WorkAndRejectProductionDemo()
    {
        await using var database = await SqlTestDatabase.CreateAsync();
        Assert.Equal(0, await RunAsync(database, "--seed-reference", "Testing", null));
        Assert.Equal(0, await RunAsync(database, "--seed-demo", "Testing", null));
        await using var db = database.Context();
        Assert.Equal(4, await db.Roles.CountAsync());
        Assert.Empty(await db.Users.ToListAsync());
        var password = Guid.NewGuid().ToString("N") + "aA1!";
        Assert.Equal(0, await RunAsync(database, "--seed-demo", "Testing", password));
        Assert.Equal(0, await RunAsync(database, "--seed-demo", "Testing", password));
        Assert.Equal(5, await db.Users.CountAsync());
        Assert.Equal(2, await db.Restaurants.CountAsync());
        Assert.NotEqual(0, await RunAsync(database, "--seed-demo", "Production", password));
        Assert.Equal(5, await db.Users.CountAsync());
    }

    private static async Task<int> RunAsync(SqlTestDatabase database, string command, string environment, string? password)
    {
        var start = new ProcessStartInfo("dotnet")
        {
            UseShellExecute = false, RedirectStandardOutput = true, RedirectStandardError = true,
            CreateNoWindow = true
        };
        start.ArgumentList.Add(typeof(Program).Assembly.Location);
        start.ArgumentList.Add(command);
        start.Environment["ASPNETCORE_ENVIRONMENT"] = environment;
        start.Environment["DOTNET_ENVIRONMENT"] = environment;
        start.Environment["ConnectionStrings__DefaultConnection"] = database.ConnectionString;
        start.Environment.Remove("Seed__DemoPassword");
        if (password is not null) start.Environment["Seed__DemoPassword"] = password;
        using var process = Process.Start(start)!;
        // Drain both streams without logging configuration or credentials.
        var output = process.StandardOutput.ReadToEndAsync();
        var errors = process.StandardError.ReadToEndAsync();
        using var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(60));
        try { await process.WaitForExitAsync(timeout.Token); }
        catch (OperationCanceledException)
        {
            process.Kill(entireProcessTree: true);
            throw new TimeoutException("Explicit seed command did not terminate.");
        }
        await Task.WhenAll(output, errors);
        return process.ExitCode;
    }
}
