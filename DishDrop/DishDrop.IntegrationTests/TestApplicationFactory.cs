using DishDrop.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DishDrop.IntegrationTests
{
    internal sealed class TestApplicationFactory : WebApplicationFactory<DishDrop.Web.Program>
    {
        private const string DummyConnectionString =
            "Server=127.0.0.1,1;Database=DishDropHttpTests;Integrated Security=True;Connect Timeout=1;TrustServerCertificate=True";

        private readonly string connectionString;
        private readonly bool rejectDatabaseConnections;

        public TestApplicationFactory()
        {
            connectionString = DummyConnectionString;
            rejectDatabaseConnections = true;
        }

        public TestApplicationFactory(string connectionString)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(connectionString);
            this.connectionString = connectionString;
            rejectDatabaseConnections = false;
        }

        protected override IHost CreateHost(IHostBuilder builder)
        {
            // Host configuration is replayed before WebApplication.CreateBuilder returns,
            // so Program's immediate configuration validation sees this per-factory value.
            builder.UseEnvironment("Testing");
            builder.ConfigureHostConfiguration(configuration =>
                configuration.AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["ConnectionStrings:DefaultConnection"] = connectionString
                }));

            return base.CreateHost(builder);
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            if (rejectDatabaseConnections)
            {
                builder.ConfigureServices(services =>
                    services.AddDbContext<ApplicationDbContext>(options =>
                        options.AddInterceptors(new RejectDatabaseConnections())));
            }
        }

        private sealed class RejectDatabaseConnections : DbConnectionInterceptor
        {
            public override InterceptionResult ConnectionOpening(
                System.Data.Common.DbConnection connection,
                ConnectionEventData eventData,
                InterceptionResult result)
            {
                throw new InvalidOperationException("HTTP tests without a database must not open a database connection.");
            }

            public override ValueTask<InterceptionResult> ConnectionOpeningAsync(
                System.Data.Common.DbConnection connection,
                ConnectionEventData eventData,
                InterceptionResult result,
                CancellationToken cancellationToken = default)
            {
                throw new InvalidOperationException("HTTP tests without a database must not open a database connection.");
            }
        }
    }
}
