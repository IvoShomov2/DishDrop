using DishDrop.Data;
using DishDrop.Data.Entities;
using DishDrop.Data.Enums;
using DishDrop.Data.Seeding;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DishDrop.IntegrationTests.Database
{
    [Trait("Category", "Database")]
    public class DatabaseSchemaTests
    {
        private const string LegacyMigration = "00000000000000_CreateIdentitySchema";

        private static string EphemeralPassword() => Guid.NewGuid().ToString("N") + "aA1!";

        private static async Task SeedAsync(SqlTestDatabase database, bool demo = true)
        {
            await using var services = database.Services();
            await using var scope = services.CreateAsyncScope();
            await scope.ServiceProvider.GetRequiredService<DatabaseSeeder>().SeedAsync(demo, demo ? EphemeralPassword() : null);
        }

        [Fact]
        public async Task EmptyDatabase_MigratesAndHasExpectedModelWithoutShadowForeignKeys()
        {
            await using var database = await SqlTestDatabase.CreateAsync();
            await using var db = database.Context();
            Assert.Equal(2, (await db.Database.GetAppliedMigrationsAsync()).Count());
            Assert.False(db.Database.HasPendingModelChanges());
            Assert.DoesNotContain(db.Model.GetEntityTypes().SelectMany(x => x.GetForeignKeys())
                .SelectMany(x => x.Properties), x => x.IsShadowProperty());
            var domain = db.Model.GetEntityTypes().Where(x => x.ClrType.Namespace == typeof(Order).Namespace);
            Assert.All(domain.SelectMany(x => x.GetForeignKeys()), fk => Assert.Equal(DeleteBehavior.NoAction, fk.DeleteBehavior));
            Assert.NotEmpty(await db.Database.SqlQueryRaw<string>("SELECT CONVERT(nvarchar(128), SERVERPROPERTY('ProductVersion')) AS [Value]").SingleAsync());
        }

        [Fact]
        public async Task LegacyIdentityUpgrade_PreservesUserAndBackfillsWithoutInventedNames()
        {
            await using var database = await SqlTestDatabase.CreateAsync(migrate: false);
            await using var db = database.Context();
            await db.GetService<IMigrator>().MigrateAsync(LegacyMigration);
            await db.Database.ExecuteSqlRawAsync("""
                INSERT INTO AspNetUsers (Id, UserName, NormalizedUserName, Email, NormalizedEmail,
                    EmailConfirmed, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnabled, AccessFailedCount, PasswordHash)
                VALUES (N'legacy-test', N'legacy@example.com', N'LEGACY@EXAMPLE.COM', N'legacy@example.com', N'LEGACY@EXAMPLE.COM',
                    1, 0, 0, 0, 0, N'preserved-test-hash')
                """);
            await db.Database.MigrateAsync();
            var user = await db.Users.SingleAsync();
            Assert.Equal("legacy-test", user.Id);
            Assert.Equal("preserved-test-hash", user.PasswordHash);
            Assert.True(user.EmailConfirmed);
            Assert.True(user.IsActive);
            Assert.Equal("", user.FirstName);
            Assert.Equal("", user.LastName);
            Assert.True(user.CreatedAtUtc > DateTime.UtcNow.AddMinutes(-5));
        }

        [Fact]
        public async Task DuplicateLegacyEmails_StopUpgradeWithDiagnosticWithoutDeletingUsers()
        {
            await using var database = await SqlTestDatabase.CreateAsync(false);
            await using var db = database.Context();
            await db.GetService<IMigrator>().MigrateAsync(LegacyMigration);
            foreach (var id in new[] { "duplicate-1", "duplicate-2" })
                await db.Database.ExecuteSqlInterpolatedAsync($"""
                    INSERT INTO AspNetUsers (Id, Email, NormalizedEmail, EmailConfirmed, PhoneNumberConfirmed,
                        TwoFactorEnabled, LockoutEnabled, AccessFailedCount)
                    VALUES ({id}, N'duplicate@example.com', N'DUPLICATE@EXAMPLE.COM', 0, 0, 0, 0, 0)
                    """);
            var exception = await Assert.ThrowsAsync<SqlException>(() => db.Database.MigrateAsync());
            Assert.Equal(51000, exception.Number);
            Assert.Contains("Duplicate nonempty NormalizedEmail", exception.Message);
            Assert.Equal(2, await db.Database.SqlQueryRaw<int>("SELECT COUNT(*) AS [Value] FROM AspNetUsers").SingleAsync());
            Assert.Single(await db.Database.GetAppliedMigrationsAsync());
        }

        [Fact]
        public async Task ReferenceAndDemoSeeding_IsIdempotentAndNoPasswordMeansNoDemoUsers()
        {
            await using var database = await SqlTestDatabase.CreateAsync();
            await using (var services = database.Services())
            await using (var scope = services.CreateAsyncScope())
                await scope.ServiceProvider.GetRequiredService<DatabaseSeeder>().SeedAsync(true, null);
            await using var db = database.Context();
            Assert.Empty(await db.Users.ToListAsync());
            Assert.Equal(4, await db.Roles.CountAsync());
            Assert.Equal(14, await db.Allergens.CountAsync());
            await SeedAsync(database);
            var before = await CountsAsync(db);
            var originalHash = await db.Users.Where(x => x.Id == "dishdrop-demo-admin").Select(x => x.PasswordHash).SingleAsync();
            await SeedAsync(database);
            Assert.Equal(before, await CountsAsync(db));
            Assert.Equal(originalHash, await db.Users.Where(x => x.Id == "dishdrop-demo-admin").Select(x => x.PasswordHash).SingleAsync());
            Assert.Equal(2, await db.Restaurants.CountAsync());
            Assert.Equal(5, await db.Users.CountAsync());
            await using var provider = database.Services();
            await using var identityScope = provider.CreateAsyncScope();
            var manager = identityScope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var customer = (await manager.FindByIdAsync("dishdrop-demo-customer"))!;
            Assert.True((await manager.AddToRoleAsync(customer, RoleNames.Courier)).Succeeded);
            Assert.Equal(2, (await manager.GetRolesAsync(customer)).Count);
        }

        private static async Task<int[]> CountsAsync(ApplicationDbContext db) =>
            [await db.Users.CountAsync(), await db.Roles.CountAsync(), await db.Cities.CountAsync(),
                await db.DeliveryAreas.CountAsync(), await db.Restaurants.CountAsync(), await db.RestaurantStaff.CountAsync(),
                await db.RestaurantWorkingPeriods.CountAsync(), await db.CourierProfiles.CountAsync(), await db.Menus.CountAsync(),
                await db.Dishes.CountAsync(), await db.DishVariants.CountAsync(), await db.DishOptions.CountAsync(),
                await db.Allergens.CountAsync(), await db.DishAllergens.CountAsync(), await db.DishOptionAllergens.CountAsync(),
                await db.CustomerAddresses.CountAsync(), await db.Carts.CountAsync(), await db.RestaurantApprovalHistories.CountAsync(),
                await db.CourierApprovalHistories.CountAsync(), await db.RestaurantDeliveryAreas.CountAsync(),
                await db.CourierDeliveryAreas.CountAsync(), await db.RestaurantCuisineTypes.CountAsync()];

        [Fact]
        public async Task CompositeUserLinks_SupportOriginalIdentityKeyLength()
        {
            await using var database = await SqlTestDatabase.CreateAsync();
            await SeedAsync(database);
            await using var db = database.Context();
            var user = new ApplicationUser { Id = new string('u', 450) };
            var courier = new CourierProfile { User = user, UserId = user.Id };
            db.AddRange(user, courier);
            db.Add(new RestaurantStaff { RestaurantId = (await db.Restaurants.FirstAsync()).Id, User = user, UserId = user.Id });
            db.Add(new CourierDeliveryArea { Courier = courier, CourierId = user.Id, DeliveryAreaId = (await db.DeliveryAreas.FirstAsync()).Id });
            await db.SaveChangesAsync();
            Assert.True(await db.RestaurantStaff.AnyAsync(x => x.UserId == user.Id));
            Assert.True(await db.CourierDeliveryAreas.AnyAsync(x => x.CourierId == user.Id));
        }

        [Fact]
        public async Task FilteredIndexes_AllowNondefaultAddressesAndEmptyLegacyEmails()
        {
            await using var database = await SqlTestDatabase.CreateAsync();
            await SeedAsync(database);
            await using var db = database.Context();
            var areaId = (await db.DeliveryAreas.FirstAsync()).Id;
            for (var i = 0; i < 2; i++)
            {
                db.Add(new ApplicationUser { Id = Guid.NewGuid().ToString(), NormalizedEmail = "" });
                db.Add(new ApplicationUser { Id = Guid.NewGuid().ToString(), NormalizedEmail = null });
                db.Add(new CustomerAddress { UserId = "dishdrop-demo-customer", DeliveryAreaId = areaId, Label = "Other", StreetAddress = "Other" });
            }
            await db.SaveChangesAsync();
            Assert.Equal(3, await db.CustomerAddresses.CountAsync());
        }

        [Theory]
        [InlineData("DefaultAddress", 2601, "IX_CustomerAddress_UserId")]
        [InlineData("RestaurantSlug", 2601, "IX_Restaurant_Slug")]
        [InlineData("Email", 2601, "EmailIndex")]
        [InlineData("ForeignKey", 547, "FK_DeliveryArea_City_CityId")]
        [InlineData("CompositePrimaryKey", 2627, "PK_RestaurantStaff")]
        [InlineData("VariantPrice", 547, "CK_DishVariant_Price")]
        [InlineData("VariantPortion", 547, "CK_DishVariant_Portion")]
        [InlineData("OptionPrice", 547, "CK_DishOption_Price")]
        [InlineData("CartQuantity", 547, "CK_CartItem_Quantity")]
        [InlineData("Rating", 547, "CK_Review_Ratings")]
        [InlineData("WorkingPeriod", 547, "CK_RestaurantWorkingPeriod_Period")]
        [InlineData("OrderTotal", 547, "CK_Order_Amounts")]
        [InlineData("OrderItemTotal", 547, "CK_OrderItem_Amounts")]
        [InlineData("EnumDomain", 547, "CK_Restaurant_ApprovalStatus")]
        [InlineData("PaymentUnique", 2601, "IX_Payment_OrderId")]
        [InlineData("PaymentAmount", 547, "CK_Payment_Amount")]
        [InlineData("OrderNumber", 2601, "IX_Order_OrderNumber")]
        [InlineData("CheckoutToken", 2601, "IX_Order_CheckoutToken")]
        [InlineData("ReviewUnique", 2601, "IX_Review_OrderId")]
        [InlineData("OptionSelectionLimits", 547, "CK_DishOptionGroup_Selections")]
        [InlineData("ScheduleExceptionUnique", 2601, "IX_RestaurantScheduleException_RestaurantId_Date")]
        [InlineData("AssignmentReleaseTime", 547, "CK_DeliveryAssignmentHistory_Release")]
        public async Task SqlServer_RejectsInvalidData(
            string scenario,
            int expectedErrorNumber,
            string expectedSqlObject)
        {
            await using var database = await SqlTestDatabase.CreateAsync();
            await SeedAsync(database);
            await using var db = database.Context();
            var restaurant = await db.Restaurants.FirstAsync();
            var variant = await db.DishVariants.FirstAsync();
            var area = await db.DeliveryAreas.FirstAsync();
            var order = await AddOrderAsync(db);
            var tables = db.GetService<IDesignTimeModel>()
                .Model.GetRelationalModel().Tables;
            Assert.Contains(tables, table =>
                table.UniqueConstraints.Any(k => k.Name == expectedSqlObject)
                || table.Indexes.Any(i => i.Name == expectedSqlObject)
                || table.ForeignKeyConstraints.Any(f => f.Name == expectedSqlObject)
                || table.CheckConstraints.Any(c => c.Name == expectedSqlObject));

            switch (scenario)
            {
                case "DefaultAddress": db.Add(new CustomerAddress { UserId = "dishdrop-demo-customer", DeliveryAreaId = area.Id, Label = "Second", StreetAddress = "Test", IsDefault = true }); break;
                case "RestaurantSlug": db.Add(new Restaurant { CityId = restaurant.CityId, Name = "Duplicate", Slug = restaurant.Slug, Description = "Test restaurant", Address = "Test", PhoneNumber = "000", EstimatedPreparationMinutes = 1 }); break;
                case "Email": db.Add(new ApplicationUser { UserName = "other@example.com", NormalizedUserName = "OTHER@EXAMPLE.COM", Email = "customer@example.com", NormalizedEmail = "CUSTOMER@EXAMPLE.COM" }); break;
                case "ForeignKey": db.Add(new DeliveryArea { CityId = int.MaxValue, Name = "Invalid", NormalizedName = "INVALID" }); break;
                case "CompositePrimaryKey":
                    var staff = await db.RestaurantStaff.AsNoTracking().FirstAsync();
                    db.Add(new RestaurantStaff { RestaurantId = staff.RestaurantId, UserId = staff.UserId }); break;
                case "VariantPrice": variant.Price = 0; break;
                case "VariantPortion": variant.PortionUnit = null; break;
                case "OptionPrice": (await db.DishOptions.FirstAsync()).AdditionalPrice = -1; break;
                case "CartQuantity": db.Add(new CartItem { CartId = (await db.Carts.FirstAsync()).Id, DishVariantId = variant.Id, Quantity = 100 }); break;
                case "Rating": db.Add(new Review { OrderId = order.Id, RestaurantRating = 6 }); break;
                case "WorkingPeriod": (await db.RestaurantWorkingPeriods.FirstAsync()).ClosesAt = new TimeOnly(9, 0); break;
                case "OrderTotal": order.TotalAmount = 999; break;
                case "OrderItemTotal": (await db.OrderItems.FirstAsync()).LineTotal = 999; break;
                case "EnumDomain": restaurant.ApprovalStatus = (ApprovalStatus)42; break;
                case "PaymentUnique": db.Add(new Payment { OrderId = order.Id, Amount = 13 }); break;
                case "PaymentAmount": (await db.Payments.SingleAsync()).Amount = -1; break;
                case "OrderNumber":
                    var numberedOrder = await AddOrderAsync(db);
                    numberedOrder.OrderNumber = order.OrderNumber; break;
                case "CheckoutToken":
                    var tokenOrder = await AddOrderAsync(db);
                    tokenOrder.CheckoutToken = order.CheckoutToken; break;
                case "ReviewUnique":
                    db.Add(new Review { OrderId = order.Id, RestaurantRating = 5 });
                    await db.SaveChangesAsync();
                    db.Add(new Review { OrderId = order.Id, RestaurantRating = 4 }); break;
                case "OptionSelectionLimits": (await db.DishOptionGroups.FirstAsync()).MaxSelections = 0; break;
                case "ScheduleExceptionUnique":
                    db.Add(new RestaurantScheduleException { RestaurantId = restaurant.Id, Date = new DateOnly(2026, 1, 1), IsClosed = true });
                    await db.SaveChangesAsync();
                    db.Add(new RestaurantScheduleException { RestaurantId = restaurant.Id, Date = new DateOnly(2026, 1, 1), IsClosed = true }); break;
                case "AssignmentReleaseTime":
                    var delivery = new Delivery { OrderId = order.Id, DeliveryAreaId = area.Id };
                    db.Add(delivery);
                    await db.SaveChangesAsync();
                    db.Add(new DeliveryAssignmentHistory { DeliveryId = delivery.Id, CourierId = "dishdrop-demo-courier",
                        AssignedAtUtc = DateTime.UtcNow, ReleasedAtUtc = DateTime.UtcNow.AddHours(-1) }); break;
                default: throw new ArgumentOutOfRangeException(nameof(scenario), scenario, "Unknown SQL constraint scenario.");
            }
            var exception = await Assert.ThrowsAsync<DbUpdateException>(() => db.SaveChangesAsync());
            var sqlException = Assert.IsType<SqlException>(exception.InnerException);
            Assert.Equal(expectedErrorNumber, sqlException.Number);
            Assert.Contains(expectedSqlObject, sqlException.Message, StringComparison.Ordinal);
        }

        [Fact]
        public async Task ActiveDeliveryAndOpenAssignmentIndexes_AreEnforcedBySqlServer()
        {
            await using var database = await SqlTestDatabase.CreateAsync();
            await SeedAsync(database);
            await using var db = database.Context();
            var order1 = await AddOrderAsync(db);
            var order2 = await AddOrderAsync(db);
            var area = await db.DeliveryAreas.FirstAsync();
            var courier = "dishdrop-demo-courier";
            var first = new Delivery { OrderId = order1.Id, DeliveryAreaId = area.Id, CourierId = courier, Status = DeliveryStatus.Assigned };
            db.Add(first);
            await db.SaveChangesAsync();
            var second = new Delivery { OrderId = order2.Id, DeliveryAreaId = area.Id, CourierId = courier, Status = DeliveryStatus.PickedUp };
            db.Add(second);
            var duplicate = await Assert.ThrowsAsync<DbUpdateException>(() => db.SaveChangesAsync());
            Assert.Equal(2601, Assert.IsType<SqlException>(duplicate.InnerException).Number);
            db.Entry(second).State = EntityState.Detached;
            first.Status = DeliveryStatus.Delivered;
            await db.SaveChangesAsync();
            db.Add(second);
            await db.SaveChangesAsync(); // Completed deliveries do not occupy the active slot.
            db.Add(new DeliveryAssignmentHistory { DeliveryId = second.Id, CourierId = courier, AssignedAtUtc = DateTime.UtcNow });
            await db.SaveChangesAsync();
            var next = new DeliveryAssignmentHistory { DeliveryId = second.Id, CourierId = courier, AssignedAtUtc = DateTime.UtcNow };
            db.Add(next);
            var assignmentDuplicate = await Assert.ThrowsAsync<DbUpdateException>(() => db.SaveChangesAsync());
            Assert.Equal(2601, Assert.IsType<SqlException>(assignmentDuplicate.InnerException).Number);
            db.Entry(next).State = EntityState.Detached;
            var openAssignment = await db.DeliveryAssignmentHistories.SingleAsync(x => x.DeliveryId == second.Id);
            openAssignment.ReleasedAtUtc = DateTime.UtcNow;
            await db.SaveChangesAsync();
            db.Add(next);
            await db.SaveChangesAsync(); // Closed assignments do not occupy the open assignment slot.
            second.CourierId = null;
            var missingCourier = await Assert.ThrowsAsync<DbUpdateException>(() => db.SaveChangesAsync());
            Assert.Equal(547, Assert.IsType<SqlException>(missingCourier.InnerException).Number);
        }

        [Fact]
        public async Task SnapshotHistoryAndRestrictDelete_SurviveCatalogueAndProfileEdits()
        {
            await using var database = await SqlTestDatabase.CreateAsync();
            await SeedAsync(database);
            await using var db = database.Context();
            var order = await AddOrderAsync(db);
            var item = await db.OrderItems.SingleAsync();
            var variant = await db.DishVariants.FindAsync(item.DishVariantId);
            var profileAddress = await db.CustomerAddresses.SingleAsync();
            variant!.Price = 27;
            variant.Name = "Edited";
            profileAddress.StreetAddress = "Edited address";
            (await db.Users.FindAsync("dishdrop-demo-customer"))!.FirstName = "Edited customer";
            (await db.Restaurants.FindAsync(order.RestaurantId))!.Address = "Edited pickup";
            (await db.DishOptions.FirstAsync(x => x.Name == "Сирене")).AdditionalPrice = 99m;
            await db.SaveChangesAsync();
            db.ChangeTracker.Clear();
            var snapshot = await db.OrderDeliveryAddresses.SingleAsync();
            Assert.Equal("Snapshot street", snapshot.StreetAddress);
            item = await db.OrderItems.SingleAsync();
            Assert.Equal(9m, item.BaseUnitPrice);
            Assert.Equal("Стандартен", item.VariantName);
            Assert.Equal(10.5m, item.LineTotal);
            var option = await db.OrderItemOptions.SingleAsync();
            Assert.Equal("Сирене", option.NameSnapshot);
            Assert.Equal(1.5m, option.AdditionalPrice);
            Assert.NotEqual("Edited pickup", (await db.Orders.SingleAsync()).PickupAddressSnapshot);
            db.ChangeTracker.Clear();
            db.Remove((await db.DishVariants.FindAsync(item.DishVariantId))!);
            Assert.Equal(547, Assert.IsType<SqlException>((await Assert.ThrowsAsync<DbUpdateException>(() => db.SaveChangesAsync())).InnerException).Number);
            db.ChangeTracker.Clear();
            db.Remove((await db.Orders.FindAsync(order.Id))!);
            Assert.Equal(547, Assert.IsType<SqlException>((await Assert.ThrowsAsync<DbUpdateException>(() => db.SaveChangesAsync())).InnerException).Number);
        }

        [Fact]
        public async Task RowVersion_RejectsLostUpdate()
        {
            await using var database = await SqlTestDatabase.CreateAsync();
            await SeedAsync(database);
            await using var first = database.Context();
            await using var second = database.Context();
            var a = await first.Restaurants.FirstAsync();
            var b = await second.Restaurants.SingleAsync(x => x.Id == a.Id);
            Assert.Equal(8, a.RowVersion.Length);
            a.Name = "First writer";
            await first.SaveChangesAsync();
            b.Name = "Stale writer";
            await Assert.ThrowsAsync<DbUpdateConcurrencyException>(() => second.SaveChangesAsync());
        }

        internal static async Task<Order> AddOrderAsync(ApplicationDbContext db)
        {
            var variant = await db.DishVariants.Include(x => x.Dish).ThenInclude(x => x.MenuCategory).ThenInclude(x => x.Menu).FirstAsync(x => x.Name == "Стандартен");
            var restaurant = await db.Restaurants.FindAsync(variant.Dish.MenuCategory.Menu.RestaurantId);
            var area = await db.DeliveryAreas.FirstAsync();
            var option = await db.DishOptions.FirstAsync(x => x.DishOptionGroup.DishId == variant.DishId && x.Name == "Сирене");
            var order = new Order { OrderNumber = Guid.NewGuid().ToString("N"), CustomerId = "dishdrop-demo-customer",
                RestaurantId = restaurant!.Id, Subtotal = 10.5m, DeliveryFee = 2.5m, TotalAmount = 13m,
                CheckoutToken = Guid.NewGuid(), CheckoutRequestHash = new string('a', 64), RestaurantNameSnapshot = restaurant.Name,
                PickupCitySnapshot = "Демоград", PickupAddressSnapshot = restaurant.Address, PickupPhoneSnapshot = restaurant.PhoneNumber };
            db.Add(order);
            db.Add(new OrderDeliveryAddress { Order = order, DeliveryAreaId = area.Id, RecipientName = "Test Customer",
                PhoneNumber = "000", CityName = "Демоград", AreaName = area.Name, StreetAddress = "Snapshot street" });
            var item = new OrderItem { Order = order, DishVariantId = variant.Id, DishName = variant.Dish.Name,
                VariantName = variant.Name, BaseUnitPrice = 9m, OptionsUnitPrice = 1.5m, Quantity = 1, LineTotal = 10.5m };
            db.Add(item);
            db.Add(new OrderItemOption { OrderItem = item, DishOptionId = option.Id, NameSnapshot = option.Name, AdditionalPrice = 1.5m });
            db.Add(new Payment { Order = order, Amount = 13m, Method = PaymentMethod.CashOnDelivery, Status = PaymentStatus.Pending });
            db.Add(new OrderStatusHistory { Order = order, ToStatus = OrderStatus.New, ChangedAtUtc = DateTime.UtcNow, Reason = "Test fixture" });
            await db.SaveChangesAsync();
            return order;
        }
    }
}
