using System.Text.RegularExpressions;
using DishDrop.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DishDrop.IntegrationTests.Database;

[Trait("Category", "Database")]
public class MigrationScriptTests
{
    [Fact]
    public async Task DeliveredIdempotentScript_RunsTwiceWithoutSchemaOrDataLoss()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);
        while (directory is not null && !File.Exists(Path.Combine(directory.FullName, "docs/sql/initial-business-schema.sql")))
            directory = directory.Parent;
        Assert.NotNull(directory);
        var script = await File.ReadAllTextAsync(Path.Combine(directory.FullName, "docs/sql/initial-business-schema.sql"));
        await using var database = await SqlTestDatabase.CreateAsync(false);
        await using var db = database.Context();
        await db.Database.OpenConnectionAsync();
        for (var run = 0; run < 2; run++)
        {
            foreach (var batch in Regex.Split(script, @"^GO\s*$", RegexOptions.Multiline | RegexOptions.IgnoreCase))
                if (!string.IsNullOrWhiteSpace(batch)) await db.Database.ExecuteSqlRawAsync(batch);
            Assert.Equal(2, (await db.Database.GetAppliedMigrationsAsync()).Count());
            if (run == 0)
            {
                db.Users.Add(new ApplicationUser { Id = "script-preserved-user", FirstName = "Test" });
                await db.SaveChangesAsync();
            }
            Assert.Equal("Test", (await db.Users.SingleAsync()).FirstName);
        }
        Assert.False(db.Database.HasPendingModelChanges());
    }
}
