using System.Data;
using DishDrop.Data.Entities;
using DishDrop.Data.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DishDrop.Data.Seeding
{
    /// <summary>Explicit, additive seed. Never migrates, deletes, resets passwords or modifies real accounts.</summary>
    public sealed class DatabaseSeeder
    {
        private const string DemoSeedReason = "Изрично demo seeding";

        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> users;
        private readonly RoleManager<IdentityRole> roles;

        public DatabaseSeeder(
            ApplicationDbContext db,
            UserManager<ApplicationUser> users,
            RoleManager<IdentityRole> roles)
        {
            this.db = db;
            this.users = users;
            this.roles = roles;
        }

        public async Task SeedAsync(bool includeDemo, string? demoPassword, CancellationToken ct = default)
        {
            await using var transaction = await db.Database.BeginTransactionAsync(IsolationLevel.Serializable, ct);
            foreach (var role in RoleNames.All)
                if (!await roles.RoleExistsAsync(role)) Ensure(await roles.CreateAsync(new IdentityRole(role)));

            var city = await db.Cities.SingleOrDefaultAsync(x => x.NormalizedName == "ДЕМОГРАД", ct);
            if (city is null)
            {
                city = new City { Name = "Демоград", NormalizedName = "ДЕМОГРАД" };
                db.Add(city);
                await db.SaveChangesAsync(ct);
            }
            foreach (var name in new[] { "Център", "Север", "Юг" })
                if (!await db.DeliveryAreas.AnyAsync(x => x.CityId == city.Id && x.NormalizedName == name.ToUpperInvariant(), ct))
                    db.Add(new DeliveryArea { CityId = city.Id, Name = name, NormalizedName = name.ToUpperInvariant() });
            foreach (var name in new[] { "Българска", "Италианска", "Вегетарианска" })
                if (!await db.CuisineTypes.AnyAsync(x => x.NormalizedName == name.ToUpperInvariant(), ct))
                    db.Add(new CuisineType { Name = name, NormalizedName = name.ToUpperInvariant() });
            var allergens = new Dictionary<string, string>
            {
                ["GLUTEN"] = "Зърнени култури с глутен", ["CRUSTACEANS"] = "Ракообразни",
                ["EGGS"] = "Яйца", ["FISH"] = "Риба", ["PEANUTS"] = "Фъстъци", ["SOY"] = "Соя",
                ["MILK"] = "Мляко", ["NUTS"] = "Ядки", ["CELERY"] = "Целина", ["MUSTARD"] = "Синап",
                ["SESAME"] = "Сусам", ["SULPHITES"] = "Сулфити", ["LUPIN"] = "Лупина", ["MOLLUSCS"] = "Мекотели"
            };
            foreach (var (code, name) in allergens)
                if (!await db.Allergens.AnyAsync(x => x.Code == code, ct)) db.Add(new Allergen { Code = code, Name = name });
            await db.SaveChangesAsync(ct);

            if (includeDemo && !string.IsNullOrWhiteSpace(demoPassword))
                await SeedDemoAsync(city, demoPassword, ct);
            await transaction.CommitAsync(ct);
        }

        private async Task SeedDemoAsync(City city, string password, CancellationToken ct)
        {
            var restaurantSpecifications = new[]
            {
                (Slug: "demo-garden", Name: "Демо Градина", CuisineName: "ВЕГЕТАРИАНСКА"),
                (Slug: "demo-oven", Name: "Демо Пещ", CuisineName: "ИТАЛИАНСКА")
            };
            var slugs = restaurantSpecifications.Select(r => r.Slug).ToArray();
            var existingRestaurants = await db.Restaurants
                .Where(r => slugs.Contains(r.Slug))
                .ToListAsync(ct);

            // Validate every collision before creating users, staff or demo content.
            // The surrounding Serializable transaction also rolls back reference seeding.
            foreach (var restaurant in existingRestaurants)
            {
                await VerifyDemoRestaurantOwnershipAsync(restaurant, city.Id, ct);
            }

            var customer = await DemoUserAsync("customer", RoleNames.Customer, password);
            var owner = await DemoUserAsync("restaurant", RoleNames.Restaurant, password);
            var employee = await DemoUserAsync("employee", RoleNames.Restaurant, password);
            var courierUser = await DemoUserAsync("courier", RoleNames.Courier, password);
            var admin = await DemoUserAsync("admin", RoleNames.Admin, password);
            var area = await db.DeliveryAreas.FirstAsync(x => x.CityId == city.Id && x.NormalizedName == "ЦЕНТЪР", ct);
            if (!await db.CustomerAddresses.AnyAsync(x => x.UserId == customer.Id, ct))
                db.Add(new CustomerAddress { UserId = customer.Id, DeliveryAreaId = area.Id, Label = "Демо адрес",
                    StreetAddress = "Измислена улица 10", IsDefault = true });
            if (!await db.Carts.AnyAsync(x => x.UserId == customer.Id, ct)) db.Add(new Cart { UserId = customer.Id });
            var courier = await db.CourierProfiles.FindAsync([courierUser.Id], ct);
            if (courier is null)
            {
                courier = new CourierProfile { UserId = courierUser.Id, ApprovalStatus = ApprovalStatus.Approved,
                    VehicleType = VehicleType.Bicycle, IsAvailable = true, ApprovedAtUtc = DateTime.UtcNow };
                db.Add(courier);
                db.Add(new CourierApprovalHistory { Courier = courier, ToStatus = ApprovalStatus.Approved,
                    ChangedByUserId = admin.Id, ChangedAtUtc = DateTime.UtcNow, Reason = DemoSeedReason });
            }
            if (!await db.CourierDeliveryAreas.AnyAsync(x => x.CourierId == courierUser.Id && x.DeliveryAreaId == area.Id, ct))
                db.Add(new CourierDeliveryArea { Courier = courier, DeliveryAreaId = area.Id });
            await db.SaveChangesAsync(ct);

            foreach (var (slug, name, cuisineName) in restaurantSpecifications)
            {
                var restaurant = await db.Restaurants.SingleOrDefaultAsync(r => r.Slug == slug, ct);
                if (restaurant is null)
                {
                    restaurant = new Restaurant { CityId = city.Id, Slug = slug, Name = name,
                        Description = "Измислен ресторант за демонстрация", Address = "Измислен площад 1",
                        PhoneNumber = "+359000000000", ApprovalStatus = ApprovalStatus.Approved,
                        IsAcceptingOrders = true, MinimumOrderAmount = 8m, EstimatedPreparationMinutes = 25 };
                    db.Add(restaurant);
                    db.Add(new RestaurantApprovalHistory { Restaurant = restaurant, ToStatus = ApprovalStatus.Approved,
                        ChangedByUserId = admin.Id, ChangedAtUtc = DateTime.UtcNow, Reason = DemoSeedReason });
                    await db.SaveChangesAsync(ct);
                }
                foreach (var (user, role) in new[] { (owner, StaffRole.Owner), (employee, StaffRole.Employee) })
                    if (!await db.RestaurantStaff.AnyAsync(x => x.RestaurantId == restaurant.Id && x.UserId == user.Id, ct))
                        db.Add(new RestaurantStaff { RestaurantId = restaurant.Id, UserId = user.Id, StaffRole = role });
                if (!await db.RestaurantWorkingPeriods.AnyAsync(x => x.RestaurantId == restaurant.Id, ct))
                    foreach (var day in Enum.GetValues<DayOfWeek>())
                        db.Add(new RestaurantWorkingPeriod { RestaurantId = restaurant.Id, DayOfWeek = day,
                            OpensAt = new TimeOnly(10, 0), ClosesAt = new TimeOnly(22, 0) });
                if (!await db.RestaurantDeliveryAreas.AnyAsync(x => x.RestaurantId == restaurant.Id && x.DeliveryAreaId == area.Id, ct))
                    db.Add(new RestaurantDeliveryArea { RestaurantId = restaurant.Id, DeliveryAreaId = area.Id, DeliveryFee = 2.5m });
                var cuisine = await db.CuisineTypes.SingleAsync(x => x.NormalizedName == cuisineName, ct);
                if (!await db.RestaurantCuisineTypes.AnyAsync(x => x.RestaurantId == restaurant.Id && x.CuisineTypeId == cuisine.Id, ct))
                    db.Add(new RestaurantCuisineType { RestaurantId = restaurant.Id, CuisineTypeId = cuisine.Id });
                await SeedMenuAsync(restaurant, ct);
            }
            await db.SaveChangesAsync(ct);
        }

        private async Task VerifyDemoRestaurantOwnershipAsync(
            Restaurant restaurant,
            int demoCityId,
            CancellationToken ct)
        {
            var ownerId = DemoUserId("restaurant");
            var adminId = DemoUserId("admin");
            var ownerEmail = users.NormalizeEmail("restaurant@example.com");
            var adminEmail = users.NormalizeEmail("admin@example.com");
            var ownerName = users.NormalizeName("restaurant@example.com");
            var adminName = users.NormalizeName("admin@example.com");

            // Stable reserved user IDs and recorded ownership/creation provenance are
            // required together. A matching slug, name, city or owner alone is insufficient.
            var hasDemoOwner = await db.RestaurantStaff
                .AnyAsync(s => s.RestaurantId == restaurant.Id
                    && s.UserId == ownerId
                    && s.StaffRole == StaffRole.Owner
                    && s.IsActive
                    && s.User.NormalizedEmail == ownerEmail
                    && s.User.NormalizedUserName == ownerName, ct);
            var hasForeignOwner = await db.RestaurantStaff
                .AnyAsync(s => s.RestaurantId == restaurant.Id
                    && s.StaffRole == StaffRole.Owner
                    && s.IsActive
                    && s.UserId != ownerId, ct);
            var hasSeedCreationHistory = await db.RestaurantApprovalHistories
                .AnyAsync(h => h.RestaurantId == restaurant.Id
                    && h.FromStatus == null
                    && h.ToStatus == ApprovalStatus.Approved
                    && h.ChangedByUserId == adminId
                    && h.ChangedByUser.NormalizedEmail == adminEmail
                    && h.ChangedByUser.NormalizedUserName == adminName
                    && h.Reason == DemoSeedReason, ct);

            if (restaurant.CityId != demoCityId || !hasDemoOwner
                || hasForeignOwner || !hasSeedCreationHistory)
            {
                throw new InvalidOperationException(
                    $"Demo seeding stopped: restaurant '{restaurant.Slug}' already exists, "
                    + "but its demo ownership and seed creation history cannot be verified. "
                    + "No seed changes were committed.");
            }
        }

        private static string DemoUserId(string handle)
        {
            return $"dishdrop-demo-{handle}";
        }

        private async Task SeedMenuAsync(Restaurant restaurant, CancellationToken ct)
        {
            // Existing demo menu is preserved; a repeat never overwrites catalogue edits.
            if (await db.Menus.AnyAsync(x => x.RestaurantId == restaurant.Id && x.Name == "Демо меню", ct)) return;
            var menu = new Menu { RestaurantId = restaurant.Id, Name = "Демо меню", IsActive = true };
            var category = new MenuCategory { Menu = menu, Name = "Основни", NormalizedName = "ОСНОВНИ", IsActive = true };
            var dish = new Dish { MenuCategory = category, Name = "Зеленчукова пица", Description = "Демо ястие" };
            db.AddRange(menu, category, dish);
            db.AddRange(
                new DishVariant { Dish = dish, Name = "Стандартен", NormalizedName = "СТАНДАРТЕН", Price = 9m,
                    PortionQuantity = 350m, PortionUnit = PortionUnit.Grams },
                new DishVariant { Dish = dish, Name = "Голям", NormalizedName = "ГОЛЯМ", Price = 12m,
                    PortionQuantity = 500m, PortionUnit = PortionUnit.Grams, DisplayOrder = 1 });
            var group = new DishOptionGroup { Dish = dish, Name = "Добавки", MinSelections = 0, MaxSelections = 2, IsActive = true };
            var cheese = new DishOption { DishOptionGroup = group, Name = "Сирене", AdditionalPrice = 1.5m };
            db.AddRange(group, cheese, new DishOption { DishOptionGroup = group, Name = "Гъби", AdditionalPrice = 1m });
            var gluten = await db.Allergens.SingleAsync(x => x.Code == "GLUTEN", ct);
            var milk = await db.Allergens.SingleAsync(x => x.Code == "MILK", ct);
            db.Add(new DishAllergen { Dish = dish, AllergenId = gluten.Id });
            db.Add(new DishOptionAllergen { DishOption = cheese, AllergenId = milk.Id });
            await db.SaveChangesAsync(ct);
        }

        private async Task<ApplicationUser> DemoUserAsync(string handle, string role, string password)
        {
            var id = DemoUserId(handle);
            var user = await users.FindByIdAsync(id);
            if (user is null)
            {
                var email = $"{handle}@example.com";
                if (await users.FindByEmailAsync(email) is not null)
                    throw new InvalidOperationException("A demo email is occupied by a non-demo account; no account was modified.");
                user = new ApplicationUser { Id = id, UserName = email, Email = email,
                    FirstName = "Демо", LastName = handle, EmailConfirmed = true };
                Ensure(await users.CreateAsync(user, password));
            }
            if (!await users.IsInRoleAsync(user, role)) Ensure(await users.AddToRoleAsync(user, role));
            return user;
        }

        private static void Ensure(IdentityResult result)
        {
            if (!result.Succeeded)
                throw new InvalidOperationException("Seed Identity operation failed: " + string.Join(", ", result.Errors.Select(x => x.Code)));
        }
    }
}
