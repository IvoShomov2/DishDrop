using DishDrop.Data;
using DishDrop.Data.Entities;
using DishDrop.Data.Seeding;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DishDrop.IntegrationTests.Database;

/// <summary>Creates and disposes only a newly generated, uniquely named database, never the application DB.</summary>
internal sealed class SqlTestDatabase : IAsyncDisposable
{
    private readonly string masterConnection;
    private bool created;
    public string Name { get; } = "DishDropTests_" + Guid.NewGuid().ToString("N");
    public string ConnectionString { get; }

    private SqlTestDatabase()
    {
        var configured = Environment.GetEnvironmentVariable("DISHDROP_TEST_SQL");
        if (string.IsNullOrWhiteSpace(configured) && !OperatingSystem.IsWindows())
            throw new InvalidOperationException("Database tests require DISHDROP_TEST_SQL pointing to a disposable SQL Server. Tests are not skipped.");
        var builder = new SqlConnectionStringBuilder(configured ?? @"Server=(localdb)\MSSQLLocalDB;Integrated Security=true;TrustServerCertificate=true")
        {
            InitialCatalog = "master", AttachDBFilename = "", Pooling = false
        };
        masterConnection = builder.ConnectionString;
        builder.InitialCatalog = Name;
        ConnectionString = builder.ConnectionString;
    }

    public static async Task<SqlTestDatabase> CreateAsync(bool migrate = true)
    {
        var database = new SqlTestDatabase();
        try
        {
            await using var master = new SqlConnection(database.masterConnection);
            await master.OpenAsync();
            await using var command = master.CreateCommand();
            // Name is exclusively generated above, and CREATE fails if it somehow already exists.
            command.CommandText = $"CREATE DATABASE [{database.Name}]";
            await command.ExecuteNonQueryAsync();
            database.created = true;
            if (migrate)
            {
                await using var db = database.Context();
                await db.Database.MigrateAsync();
            }
            return database;
        }
        catch
        {
            await database.DisposeAsync();
            throw;
        }
    }

    public ApplicationDbContext Context() => new(new DbContextOptionsBuilder<ApplicationDbContext>()
        .UseSqlServer(ConnectionString).Options);

    public ServiceProvider Services()
    {
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddDbContext<ApplicationDbContext>(o => o.UseSqlServer(ConnectionString));
        services.AddIdentityCore<ApplicationUser>(o => o.User.RequireUniqueEmail = true)
            .AddRoles<IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();
        services.AddScoped<DatabaseSeeder>();
        return services.BuildServiceProvider();
    }

    public async ValueTask DisposeAsync()
    {
        if (!created) return;
        await using var master = new SqlConnection(masterConnection);
        await master.OpenAsync();
        await using var command = master.CreateCommand();
        command.CommandText = $"ALTER DATABASE [{Name}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE; DROP DATABASE [{Name}]";
        await command.ExecuteNonQueryAsync();
        created = false;
    }
}
