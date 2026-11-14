using System.Security.Cryptography;
using System.Text;
using DishDrop.Data.Entities;
using DishDrop.Data.Enums;
using DishDrop.Data.Seeding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DishDrop.IntegrationTests.Database
{
    [Trait("Category", "Database")]
    public class DemoSeedingSafetyTests
    {
        [Theory]
        [InlineData("demo-garden", true)]
        [InlineData("demo-garden", false)]
        [InlineData("demo-oven", true)]
        [InlineData("demo-oven", false)]
        public async Task ForeignSlugCollision_PreservesAllDataAndRollsBackEntireSeed(
            string slug,
            bool sameCity)
        {
            await using var database = await SqlTestDatabase.CreateAsync();
            var restaurantId = await AddForeignRestaurantAsync(database, slug, sameCity);
            var before = await CaptureDatabaseStateAsync(database);

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => SeedAsync(database));

            Assert.Contains(slug, exception.Message);
            Assert.Contains("demo ownership and seed creation history", exception.Message);
            Assert.Equal(before, await CaptureDatabaseStateAsync(database));

            await using var db = database.Context();
            Assert.Single(await db.Restaurants.ToListAsync());
            Assert.Equal("foreign-owner", (await db.RestaurantStaff.SingleAsync()).UserId);
            Assert.Equal("Existing menu", (await db.Menus.SingleAsync()).Name);
            Assert.Empty(await db.RestaurantStaff
                .Where(s => s.RestaurantId == restaurantId && s.UserId.StartsWith("dishdrop-demo-"))
                .ToListAsync());
            Assert.Empty(await db.Users.Where(u => u.Id.StartsWith("dishdrop-demo-")).ToListAsync());
            Assert.Empty(await db.Roles.ToListAsync());
            Assert.Empty(await db.Allergens.ToListAsync());
        }

        [Theory]
        [InlineData("MissingOwner")]
        [InlineData("MissingSeedHistory")]
        [InlineData("ForeignCoOwner")]
        public async Task IncompleteDemoProvenance_IsRejectedWithoutChanges(string scenario)
        {
            await using var database = await SqlTestDatabase.CreateAsync();
            await SeedAsync(database);

            await using (var db = database.Context())
            {
                var restaurant = await db.Restaurants.SingleAsync(r => r.Slug == "demo-garden");
                switch (scenario)
                {
                    case "MissingOwner":
                        (await db.RestaurantStaff.SingleAsync(s => s.RestaurantId == restaurant.Id
                            && s.UserId == "dishdrop-demo-restaurant")).IsActive = false;
                        break;
                    case "MissingSeedHistory":
                        (await db.RestaurantApprovalHistories.SingleAsync(h => h.RestaurantId == restaurant.Id))
                            .Reason = "Manual approval, not a seed creation event";
                        break;
                    case "ForeignCoOwner":
                        var owner = new ApplicationUser
                        {
                            Id = "foreign-owner",
                            FirstName = "Existing",
                            LastName = "Owner"
                        };
                        db.Add(owner);
                        db.Add(new RestaurantStaff
                        {
                            RestaurantId = restaurant.Id,
                            User = owner,
                            UserId = owner.Id,
                            StaffRole = StaffRole.Owner
                        });
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(scenario));
                }

                await db.SaveChangesAsync();
            }

            var before = await CaptureDatabaseStateAsync(database);
            await Assert.ThrowsAsync<InvalidOperationException>(() => SeedAsync(database));
            Assert.Equal(before, await CaptureDatabaseStateAsync(database));
        }

        [Fact]
        public async Task ValidDemoProvenance_AllowsIdempotentSeedingAndPreservesDisplayEdits()
        {
            await using var database = await SqlTestDatabase.CreateAsync();
            await SeedAsync(database);

            await using (var db = database.Context())
            {
                var restaurant = await db.Restaurants.SingleAsync(r => r.Slug == "demo-garden");
                restaurant.Name = "Edited demo display name";
                restaurant.Description = "Keep this description";
                await db.SaveChangesAsync();
            }

            var before = await CaptureDatabaseStateAsync(database);
            await SeedAsync(database);
            Assert.Equal(before, await CaptureDatabaseStateAsync(database));
        }

        private static async Task SeedAsync(SqlTestDatabase database)
        {
            await using var provider = database.Services();
            await using var scope = provider.CreateAsyncScope();
            await scope.ServiceProvider.GetRequiredService<DatabaseSeeder>()
                .SeedAsync(true, Guid.NewGuid().ToString("N") + "aA1!");
        }

        private static async Task<int> AddForeignRestaurantAsync(
            SqlTestDatabase database,
            string slug,
            bool sameCity)
        {
            await using var db = database.Context();
            var city = new City
            {
                Name = sameCity ? "Демоград" : "Existing city",
                NormalizedName = sameCity ? "ДЕМОГРАД" : "EXISTING CITY"
            };
            var owner = new ApplicationUser
            {
                Id = "foreign-owner",
                FirstName = "Existing",
                LastName = "Owner"
            };
            var restaurant = new Restaurant
            {
                City = city,
                Name = slug == "demo-garden" ? "Демо Градина" : "Демо Пещ",
                Slug = slug,
                Description = "Existing restaurant; never seeded",
                Address = "Existing address",
                PhoneNumber = "000",
                EstimatedPreparationMinutes = 15
            };
            var area = new DeliveryArea { City = city, Name = "Existing area", NormalizedName = "EXISTING AREA" };
            var cuisine = new CuisineType { Name = "Existing cuisine", NormalizedName = "EXISTING CUISINE" };
            db.AddRange(city, owner, restaurant, area, cuisine);
            db.Add(new RestaurantStaff { Restaurant = restaurant, User = owner, UserId = owner.Id, StaffRole = StaffRole.Owner });
            db.Add(new Menu { Restaurant = restaurant, Name = "Existing menu", IsActive = true });
            db.Add(new RestaurantWorkingPeriod
            {
                Restaurant = restaurant,
                DayOfWeek = DayOfWeek.Monday,
                OpensAt = new TimeOnly(11, 0),
                ClosesAt = new TimeOnly(21, 0)
            });
            db.Add(new RestaurantDeliveryArea { Restaurant = restaurant, DeliveryArea = area, DeliveryFee = 3m });
            db.Add(new RestaurantCuisineType { Restaurant = restaurant, CuisineType = cuisine });
            await db.SaveChangesAsync();
            return restaurant.Id;
        }

        private static async Task<SortedDictionary<string, string>> CaptureDatabaseStateAsync(SqlTestDatabase database)
        {
            await using var db = database.Context();
            await db.Database.OpenConnectionAsync();
            var result = new SortedDictionary<string, string>(StringComparer.Ordinal);

            foreach (var table in db.Model.GetRelationalModel().Tables)
            {
                // Names come exclusively from the EF model, not from user input.
                var tableName = Quote(table.Schema ?? "dbo") + "." + Quote(table.Name);
                var order = string.Join(", ", table.PrimaryKey!.Columns.Select(c => Quote(c.Name)));
                await using var command = db.Database.GetDbConnection().CreateCommand();
                command.CommandText = $"SELECT * FROM {tableName} ORDER BY {order} FOR JSON PATH, INCLUDE_NULL_VALUES";
                var json = new StringBuilder();
                await using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync()) json.Append(reader.GetString(0));
                }

                // Hash the full persisted rows, including rowversion, without exposing account data in assertions.
                result[tableName] = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(json.ToString())));
            }

            return result;
        }

        private static string Quote(string identifier)
        {
            return "[" + identifier.Replace("]", "]]", StringComparison.Ordinal) + "]";
        }
    }
}
