using DishDrop.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DishDrop.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<City> Cities { get; set; } = null!;

        public DbSet<DeliveryArea> DeliveryAreas { get; set; } = null!;

        public DbSet<CustomerAddress> CustomerAddresses { get; set; } = null!;

        public DbSet<Restaurant> Restaurants { get; set; } = null!;

        public DbSet<RestaurantStaff> RestaurantStaff { get; set; } = null!;

        public DbSet<RestaurantWorkingPeriod> RestaurantWorkingPeriods { get; set; } = null!;

        public DbSet<RestaurantScheduleException> RestaurantScheduleExceptions { get; set; } = null!;

        public DbSet<RestaurantScheduleExceptionPeriod> RestaurantScheduleExceptionPeriods { get; set; } = null!;

        public DbSet<RestaurantDeliveryArea> RestaurantDeliveryAreas { get; set; } = null!;

        public DbSet<CuisineType> CuisineTypes { get; set; } = null!;

        public DbSet<RestaurantCuisineType> RestaurantCuisineTypes { get; set; } = null!;

        public DbSet<CourierProfile> CourierProfiles { get; set; } = null!;

        public DbSet<CourierDeliveryArea> CourierDeliveryAreas { get; set; } = null!;

        public DbSet<RestaurantApprovalHistory> RestaurantApprovalHistories { get; set; } = null!;

        public DbSet<CourierApprovalHistory> CourierApprovalHistories { get; set; } = null!;

        public DbSet<Menu> Menus { get; set; } = null!;

        public DbSet<MenuCategory> MenuCategories { get; set; } = null!;

        public DbSet<Dish> Dishes { get; set; } = null!;

        public DbSet<DishVariant> DishVariants { get; set; } = null!;

        public DbSet<DishOptionGroup> DishOptionGroups { get; set; } = null!;

        public DbSet<DishOption> DishOptions { get; set; } = null!;

        public DbSet<Allergen> Allergens { get; set; } = null!;

        public DbSet<DishAllergen> DishAllergens { get; set; } = null!;

        public DbSet<DishOptionAllergen> DishOptionAllergens { get; set; } = null!;

        public DbSet<Cart> Carts { get; set; } = null!;

        public DbSet<CartItem> CartItems { get; set; } = null!;

        public DbSet<CartItemOption> CartItemOptions { get; set; } = null!;

        public DbSet<Order> Orders { get; set; } = null!;

        public DbSet<OrderDeliveryAddress> OrderDeliveryAddresses { get; set; } = null!;

        public DbSet<OrderItem> OrderItems { get; set; } = null!;

        public DbSet<OrderItemOption> OrderItemOptions { get; set; } = null!;

        public DbSet<OrderStatusHistory> OrderStatusHistories { get; set; } = null!;

        public DbSet<Payment> Payments { get; set; } = null!;

        public DbSet<Delivery> Deliveries { get; set; } = null!;

        public DbSet<DeliveryAssignmentHistory> DeliveryAssignmentHistories { get; set; } = null!;

        public DbSet<DeliveryStatusHistory> DeliveryStatusHistories { get; set; } = null!;

        public DbSet<Review> Reviews { get; set; } = null!;

        public DbSet<ReviewReply> ReviewReplies { get; set; } = null!;

        public DbSet<SupportTicket> SupportTickets { get; set; } = null!;

        public DbSet<SupportTicketMessage> SupportTicketMessages { get; set; } = null!;

        public DbSet<Notification> Notifications { get; set; } = null!;

        public DbSet<AuditLog> AuditLogs { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Identity: preserve key lengths, defaults, email uniqueness and existing Cascade links.
            builder.Entity<IdentityUserLogin<string>>()
                .Property(l => l.LoginProvider)
                .HasMaxLength(DataConstants.Identity.LoginProviderMaxLength);

            builder.Entity<IdentityUserLogin<string>>()
                .Property(l => l.ProviderKey)
                .HasMaxLength(DataConstants.Identity.ProviderKeyMaxLength);

            builder.Entity<IdentityUserToken<string>>()
                .Property(t => t.LoginProvider)
                .HasMaxLength(DataConstants.Identity.LoginProviderMaxLength);

            builder.Entity<IdentityUserToken<string>>()
                .Property(t => t.Name)
                .HasMaxLength(DataConstants.Identity.TokenNameMaxLength);

            builder.Entity<ApplicationUser>()
                .Property(u => u.CreatedAtUtc)
                .HasDefaultValueSql("SYSUTCDATETIME()");

            builder.Entity<ApplicationUser>()
                .Property(u => u.FirstName)
                .HasDefaultValue("");

            builder.Entity<ApplicationUser>()
                .Property(u => u.LastName)
                .HasDefaultValue("");

            builder.Entity<ApplicationUser>()
                .Property(u => u.IsActive)
                .HasDefaultValue(true);

            builder.Entity<ApplicationUser>()
                .HasIndex(u => u.NormalizedEmail)
                .HasDatabaseName("EmailIndex")
                .IsUnique()
                .HasFilter(DataConstants.ApplicationUser.NonemptyEmailFilter);

            builder.Entity<IdentityUserClaim<string>>()
                .HasOne<ApplicationUser>()
                .WithMany(u => u.UserClaims)
                .HasForeignKey(c => c.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<IdentityUserLogin<string>>()
                .HasOne<ApplicationUser>()
                .WithMany(u => u.UserLogins)
                .HasForeignKey(l => l.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<IdentityUserToken<string>>()
                .HasOne<ApplicationUser>()
                .WithMany(u => u.UserTokens)
                .HasForeignKey(t => t.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<IdentityUserRole<string>>()
                .HasOne<ApplicationUser>()
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // Geography, addresses, restaurants, schedules and couriers.
            builder.Entity<City>()
                .HasIndex(c => c.NormalizedName)
                .IsUnique();

            builder.Entity<DeliveryArea>()
                .HasOne(a => a.City)
                .WithMany(c => c.DeliveryAreas)
                .HasForeignKey(a => a.CityId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<DeliveryArea>()
                .HasIndex(a => new { a.CityId, a.NormalizedName })
                .IsUnique();

            builder.Entity<CustomerAddress>()
                .HasOne(a => a.User)
                .WithMany(u => u.CustomerAddresses)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<CustomerAddress>()
                .HasOne(a => a.DeliveryArea)
                .WithMany(u => u.CustomerAddresses)
                .HasForeignKey(a => a.DeliveryAreaId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<CustomerAddress>()
                .HasIndex(a => a.UserId)
                .IsUnique()
                .HasFilter(DataConstants.CustomerAddress.DefaultAddressFilter);

            builder.Entity<Restaurant>()
                .HasOne(r => r.City)
                .WithMany(c => c.Restaurants)
                .HasForeignKey(r => r.CityId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Restaurant>()
                .HasIndex(r => r.Slug)
                .IsUnique();

            builder.Entity<Restaurant>()
                .ToTable(t => t.HasCheckConstraint("CK_Restaurant_MinimumOrder", DataConstants.Restaurant.MinimumOrderCheckSql));

            builder.Entity<Restaurant>()
                .ToTable(t => t.HasCheckConstraint("CK_Restaurant_Preparation", DataConstants.Restaurant.PreparationCheckSql));

            builder.Entity<Restaurant>()
                .ToTable(t => t.HasCheckConstraint("CK_Restaurant_ApprovalStatus", DataConstants.Restaurant.ApprovalStatusCheckSql));

            builder.Entity<RestaurantStaff>()
                .HasKey(s => new { s.RestaurantId, s.UserId })
                .IsClustered(false);

            builder.Entity<RestaurantStaff>()
                .HasOne(s => s.Restaurant)
                .WithMany(r => r.Staff)
                .HasForeignKey(s => s.RestaurantId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<RestaurantStaff>()
                .HasOne(s => s.User)
                .WithMany(u => u.RestaurantMemberships)
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<RestaurantStaff>()
                .ToTable(t => t.HasCheckConstraint("CK_RestaurantStaff_StaffRole", DataConstants.RestaurantStaff.StaffRoleCheckSql));

            builder.Entity<RestaurantWorkingPeriod>()
                .HasOne(p => p.Restaurant)
                .WithMany(r => r.WorkingPeriods)
                .HasForeignKey(p => p.RestaurantId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<RestaurantWorkingPeriod>()
                .HasIndex(p => new { p.RestaurantId, p.DayOfWeek });

            builder.Entity<RestaurantWorkingPeriod>()
                .ToTable(t => t.HasCheckConstraint("CK_RestaurantWorkingPeriod_Period", DataConstants.RestaurantWorkingPeriod.PeriodCheckSql));

            builder.Entity<RestaurantWorkingPeriod>()
                .ToTable(t => t.HasCheckConstraint("CK_RestaurantWorkingPeriod_DayOfWeek", DataConstants.RestaurantWorkingPeriod.DayOfWeekCheckSql));

            builder.Entity<RestaurantScheduleException>()
                .HasOne(e => e.Restaurant)
                .WithMany(r => r.ScheduleExceptions)
                .HasForeignKey(e => e.RestaurantId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<RestaurantScheduleException>()
                .HasIndex(e => new { e.RestaurantId, e.Date })
                .IsUnique();

            builder.Entity<RestaurantScheduleExceptionPeriod>()
                .HasOne(p => p.RestaurantScheduleException)
                .WithMany(e => e.Periods)
                .HasForeignKey(p => p.RestaurantScheduleExceptionId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<RestaurantScheduleExceptionPeriod>()
                .ToTable(t => t.HasCheckConstraint("CK_RestaurantScheduleExceptionPeriod_Period", DataConstants.RestaurantScheduleExceptionPeriod.PeriodCheckSql));

            builder.Entity<RestaurantDeliveryArea>()
                .HasKey(a => new { a.RestaurantId, a.DeliveryAreaId });

            builder.Entity<RestaurantDeliveryArea>()
                .HasOne(a => a.Restaurant)
                .WithMany(r => r.RestaurantDeliveryAreas)
                .HasForeignKey(a => a.RestaurantId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<RestaurantDeliveryArea>()
                .HasOne(a => a.DeliveryArea)
                .WithMany(r => r.RestaurantDeliveryAreas)
                .HasForeignKey(a => a.DeliveryAreaId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<RestaurantDeliveryArea>()
                .ToTable(t => t.HasCheckConstraint("CK_RestaurantDeliveryArea_Fee", DataConstants.RestaurantDeliveryArea.FeeCheckSql));

            builder.Entity<CuisineType>()
                .HasIndex(c => c.NormalizedName)
                .IsUnique();

            builder.Entity<RestaurantCuisineType>()
                .HasKey(c => new { c.RestaurantId, c.CuisineTypeId });

            builder.Entity<RestaurantCuisineType>()
                .HasOne(c => c.Restaurant)
                .WithMany(r => r.RestaurantCuisineTypes)
                .HasForeignKey(c => c.RestaurantId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<RestaurantCuisineType>()
                .HasOne(c => c.CuisineType)
                .WithMany(r => r.RestaurantCuisineTypes)
                .HasForeignKey(c => c.CuisineTypeId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<CourierProfile>()
                .HasOne(c => c.User)
                .WithOne(u => u.CourierProfile)
                .HasForeignKey<CourierProfile>(c => c.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<CourierProfile>()
                .ToTable(t => t.HasCheckConstraint("CK_CourierProfile_ApprovalStatus", DataConstants.CourierProfile.ApprovalStatusCheckSql));

            builder.Entity<CourierProfile>()
                .ToTable(t => t.HasCheckConstraint("CK_CourierProfile_VehicleType", DataConstants.CourierProfile.VehicleTypeCheckSql));

            builder.Entity<CourierDeliveryArea>()
                .HasKey(a => new { a.CourierId, a.DeliveryAreaId })
                .IsClustered(false);

            builder.Entity<CourierDeliveryArea>()
                .HasOne(a => a.Courier)
                .WithMany(c => c.DeliveryAreas)
                .HasForeignKey(a => a.CourierId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<CourierDeliveryArea>()
                .HasOne(a => a.DeliveryArea)
                .WithMany(a => a.CourierDeliveryAreas)
                .HasForeignKey(a => a.DeliveryAreaId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<RestaurantApprovalHistory>()
                .HasOne(h => h.Restaurant)
                .WithMany(r => r.ApprovalHistory)
                .HasForeignKey(h => h.RestaurantId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<RestaurantApprovalHistory>()
                .HasOne(h => h.ChangedByUser)
                .WithMany(u => u.RestaurantApprovalChanges)
                .HasForeignKey(h => h.ChangedByUserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<RestaurantApprovalHistory>()
                .HasIndex(h => new { h.RestaurantId, h.ChangedAtUtc });

            builder.Entity<RestaurantApprovalHistory>()
                .ToTable(t => t.HasCheckConstraint("CK_RestaurantApprovalHistory_FromStatus", DataConstants.RestaurantApprovalHistory.FromStatusCheckSql));

            builder.Entity<RestaurantApprovalHistory>()
                .ToTable(t => t.HasCheckConstraint("CK_RestaurantApprovalHistory_ToStatus", DataConstants.RestaurantApprovalHistory.ToStatusCheckSql));

            builder.Entity<CourierApprovalHistory>()
                .HasOne(h => h.Courier)
                .WithMany(r => r.ApprovalHistory)
                .HasForeignKey(h => h.CourierId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<CourierApprovalHistory>()
                .HasOne(h => h.ChangedByUser)
                .WithMany(u => u.CourierApprovalChanges)
                .HasForeignKey(h => h.ChangedByUserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<CourierApprovalHistory>()
                .HasIndex(h => new { h.CourierId, h.ChangedAtUtc });

            builder.Entity<CourierApprovalHistory>()
                .ToTable(t => t.HasCheckConstraint("CK_CourierApprovalHistory_FromStatus", DataConstants.CourierApprovalHistory.FromStatusCheckSql));

            builder.Entity<CourierApprovalHistory>()
                .ToTable(t => t.HasCheckConstraint("CK_CourierApprovalHistory_ToStatus", DataConstants.CourierApprovalHistory.ToStatusCheckSql));

            // Menus, variants, options and allergens.
            builder.Entity<Menu>()
                .HasOne(m => m.Restaurant)
                .WithMany(r => r.Menus)
                .HasForeignKey(m => m.RestaurantId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Menu>()
                .HasIndex(m => new { m.RestaurantId, m.DisplayOrder });

            builder.Entity<MenuCategory>()
                .HasOne(c => c.Menu)
                .WithMany(m => m.Categories)
                .HasForeignKey(c => c.MenuId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<MenuCategory>()
                .HasIndex(c => new { c.MenuId, c.NormalizedName })
                .IsUnique();

            builder.Entity<Dish>()
                .HasOne(d => d.MenuCategory)
                .WithMany(c => c.Dishes)
                .HasForeignKey(d => d.MenuCategoryId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Dish>()
                .HasIndex(d => new { d.MenuCategoryId, d.DisplayOrder });

            builder.Entity<DishVariant>()
                .HasOne(v => v.Dish)
                .WithMany(d => d.Variants)
                .HasForeignKey(v => v.DishId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<DishVariant>()
                .HasIndex(v => new { v.DishId, v.NormalizedName })
                .IsUnique();

            builder.Entity<DishVariant>()
                .ToTable(t => t.HasCheckConstraint("CK_DishVariant_Price", DataConstants.DishVariant.PriceCheckSql));

            builder.Entity<DishVariant>()
                .ToTable(t => t.HasCheckConstraint("CK_DishVariant_Portion", DataConstants.DishVariant.PortionCheckSql));

            builder.Entity<DishVariant>()
                .ToTable(t => t.HasCheckConstraint("CK_DishVariant_PortionUnit", DataConstants.DishVariant.PortionUnitCheckSql));

            builder.Entity<DishOptionGroup>()
                .HasOne(g => g.Dish)
                .WithMany(d => d.OptionGroups)
                .HasForeignKey(g => g.DishId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<DishOptionGroup>()
                .ToTable(t => t.HasCheckConstraint("CK_DishOptionGroup_Selections", DataConstants.DishOptionGroup.SelectionsCheckSql));

            builder.Entity<DishOption>()
                .HasOne(o => o.DishOptionGroup)
                .WithMany(g => g.Options)
                .HasForeignKey(o => o.DishOptionGroupId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<DishOption>()
                .ToTable(t => t.HasCheckConstraint("CK_DishOption_Price", DataConstants.DishOption.PriceCheckSql));

            builder.Entity<Allergen>()
                .HasIndex(a => a.Code)
                .IsUnique();

            builder.Entity<DishAllergen>()
                .HasKey(a => new { a.DishId, a.AllergenId });

            builder.Entity<DishAllergen>()
                .HasOne(a => a.Dish)
                .WithMany(d => d.DishAllergens)
                .HasForeignKey(a => a.DishId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<DishAllergen>()
                .HasOne(a => a.Allergen)
                .WithMany(d => d.DishAllergens)
                .HasForeignKey(a => a.AllergenId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<DishOptionAllergen>()
                .HasKey(a => new { a.DishOptionId, a.AllergenId });

            builder.Entity<DishOptionAllergen>()
                .HasOne(a => a.DishOption)
                .WithMany(o => o.DishOptionAllergens)
                .HasForeignKey(a => a.DishOptionId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<DishOptionAllergen>()
                .HasOne(a => a.Allergen)
                .WithMany(o => o.DishOptionAllergens)
                .HasForeignKey(a => a.AllergenId)
                .OnDelete(DeleteBehavior.NoAction);

            // Carts, orders, purchase snapshots and status history.
            builder.Entity<Cart>()
                .HasOne(c => c.User)
                .WithMany(u => u.Carts)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Cart>()
                .HasOne(c => c.Restaurant)
                .WithMany(u => u.Carts)
                .HasForeignKey(c => c.RestaurantId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Cart>()
                .HasIndex(c => c.UserId)
                .IsUnique();

            builder.Entity<CartItem>()
                .HasOne(i => i.Cart)
                .WithMany(c => c.Items)
                .HasForeignKey(i => i.CartId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<CartItem>()
                .HasOne(i => i.DishVariant)
                .WithMany(v => v.CartItems)
                .HasForeignKey(i => i.DishVariantId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<CartItem>()
                .ToTable(t => t.HasCheckConstraint("CK_CartItem_Quantity", DataConstants.CartItem.QuantityCheckSql));

            builder.Entity<CartItemOption>()
                .HasKey(o => new { o.CartItemId, o.DishOptionId });

            builder.Entity<CartItemOption>()
                .HasOne(o => o.CartItem)
                .WithMany(g => g.Options)
                .HasForeignKey(o => o.CartItemId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<CartItemOption>()
                .HasOne(o => o.DishOption)
                .WithMany(o => o.CartItemOptions)
                .HasForeignKey(o => o.DishOptionId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Order>()
                .HasOne(o => o.Customer)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.CustomerId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Order>()
                .HasOne(o => o.Restaurant)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.RestaurantId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Order>()
                .HasIndex(o => o.OrderNumber)
                .IsUnique();

            builder.Entity<Order>()
                .HasIndex(o => o.CheckoutToken)
                .IsUnique();

            builder.Entity<Order>()
                .HasIndex(o => new { o.CustomerId, o.CreatedAtUtc });

            builder.Entity<Order>()
                .HasIndex(o => new { o.RestaurantId, o.Status, o.CreatedAtUtc });

            builder.Entity<Order>()
                .ToTable(t => t.HasCheckConstraint("CK_Order_Amounts", DataConstants.Order.AmountsCheckSql));

            builder.Entity<Order>()
                .ToTable(t => t.HasCheckConstraint("CK_Order_Currency", DataConstants.Order.CurrencyCheckSql));

            builder.Entity<Order>()
                .ToTable(t => t.HasCheckConstraint("CK_Order_Hash", DataConstants.Order.HashCheckSql));

            builder.Entity<Order>()
                .ToTable(t => t.HasCheckConstraint("CK_Order_Status", DataConstants.Order.StatusCheckSql));

            builder.Entity<OrderDeliveryAddress>()
                .HasOne(a => a.Order)
                .WithOne(o => o.DeliveryAddress)
                .HasForeignKey<OrderDeliveryAddress>(a => a.OrderId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<OrderDeliveryAddress>()
                .HasOne(a => a.DeliveryArea)
                .WithMany(a => a.OrderDeliveryAddresses)
                .HasForeignKey(a => a.DeliveryAreaId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<OrderItem>()
                .HasOne(i => i.Order)
                .WithMany(c => c.Items)
                .HasForeignKey(i => i.OrderId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<OrderItem>()
                .HasOne(i => i.DishVariant)
                .WithMany(v => v.OrderItems)
                .HasForeignKey(i => i.DishVariantId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<OrderItem>()
                .ToTable(t => t.HasCheckConstraint("CK_OrderItem_Amounts", DataConstants.OrderItem.AmountsCheckSql));

            builder.Entity<OrderItem>()
                .ToTable(t => t.HasCheckConstraint("CK_OrderItem_Quantity", DataConstants.OrderItem.QuantityCheckSql));

            builder.Entity<OrderItemOption>()
                .HasOne(o => o.OrderItem)
                .WithMany(g => g.Options)
                .HasForeignKey(o => o.OrderItemId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<OrderItemOption>()
                .HasOne(o => o.DishOption)
                .WithMany(o => o.OrderItemOptions)
                .HasForeignKey(o => o.DishOptionId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<OrderItemOption>()
                .HasIndex(o => new { o.OrderItemId, o.DishOptionId })
                .IsUnique();

            builder.Entity<OrderItemOption>()
                .ToTable(t => t.HasCheckConstraint("CK_OrderItemOption_Price", DataConstants.OrderItemOption.PriceCheckSql));

            builder.Entity<OrderStatusHistory>()
                .HasOne(h => h.Order)
                .WithMany(o => o.StatusHistory)
                .HasForeignKey(h => h.OrderId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<OrderStatusHistory>()
                .HasOne(h => h.ChangedByUser)
                .WithMany(u => u.OrderStatusChanges)
                .HasForeignKey(h => h.ChangedByUserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<OrderStatusHistory>()
                .HasIndex(h => new { h.OrderId, h.ChangedAtUtc });

            builder.Entity<OrderStatusHistory>()
                .ToTable(t => t.HasCheckConstraint("CK_OrderStatusHistory_FromStatus", DataConstants.OrderStatusHistory.FromStatusCheckSql));

            builder.Entity<OrderStatusHistory>()
                .ToTable(t => t.HasCheckConstraint("CK_OrderStatusHistory_ToStatus", DataConstants.OrderStatusHistory.ToStatusCheckSql));

            // Cash payments, deliveries and assignment history.
            builder.Entity<Payment>()
                .HasOne(p => p.Order)
                .WithMany(o => o.Payments)
                .HasForeignKey(p => p.OrderId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Payment>()
                .HasOne(p => p.CollectedByUser)
                .WithMany(u => u.CollectedPayments)
                .HasForeignKey(p => p.CollectedByUserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Payment>()
                .HasIndex(p => p.OrderId)
                .IsUnique();

            builder.Entity<Payment>()
                .ToTable(t => t.HasCheckConstraint("CK_Payment_Amount", DataConstants.Payment.AmountCheckSql));

            builder.Entity<Payment>()
                .ToTable(t => t.HasCheckConstraint("CK_Payment_Currency", DataConstants.Payment.CurrencyCheckSql));

            builder.Entity<Payment>()
                .ToTable(t => t.HasCheckConstraint("CK_Payment_Method", DataConstants.Payment.MethodCheckSql));

            builder.Entity<Payment>()
                .ToTable(t => t.HasCheckConstraint("CK_Payment_Status", DataConstants.Payment.StatusCheckSql));

            builder.Entity<Delivery>()
                .HasOne(d => d.Order)
                .WithMany(o => o.Deliveries)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Delivery>()
                .HasOne(d => d.Courier)
                .WithMany(o => o.Deliveries)
                .HasForeignKey(d => d.CourierId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Delivery>()
                .HasOne(d => d.DeliveryArea)
                .WithMany(o => o.Deliveries)
                .HasForeignKey(d => d.DeliveryAreaId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Delivery>()
                .HasIndex(d => d.OrderId)
                .IsUnique();

            builder.Entity<Delivery>()
                .HasIndex(d => d.CourierId)
                .IsUnique()
                .HasFilter(DataConstants.Delivery.ActiveCourierFilter);

            builder.Entity<Delivery>()
                .ToTable(t => t.HasCheckConstraint("CK_Delivery_Courier", DataConstants.Delivery.CourierCheckSql));

            builder.Entity<Delivery>()
                .ToTable(t => t.HasCheckConstraint("CK_Delivery_Status", DataConstants.Delivery.StatusCheckSql));

            builder.Entity<DeliveryAssignmentHistory>()
                .HasOne(h => h.Delivery)
                .WithMany(d => d.AssignmentHistory)
                .HasForeignKey(h => h.DeliveryId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<DeliveryAssignmentHistory>()
                .HasOne(h => h.Courier)
                .WithMany(d => d.AssignmentHistory)
                .HasForeignKey(h => h.CourierId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<DeliveryAssignmentHistory>()
                .HasIndex(h => h.DeliveryId)
                .IsUnique()
                .HasFilter(DataConstants.DeliveryAssignmentHistory.OpenAssignmentFilter);

            builder.Entity<DeliveryAssignmentHistory>()
                .ToTable(t => t.HasCheckConstraint("CK_DeliveryAssignmentHistory_Release", DataConstants.DeliveryAssignmentHistory.ReleaseCheckSql));

            builder.Entity<DeliveryStatusHistory>()
                .HasOne(h => h.Delivery)
                .WithMany(o => o.StatusHistory)
                .HasForeignKey(h => h.DeliveryId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<DeliveryStatusHistory>()
                .HasOne(h => h.ChangedByUser)
                .WithMany(u => u.DeliveryStatusChanges)
                .HasForeignKey(h => h.ChangedByUserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<DeliveryStatusHistory>()
                .HasIndex(h => new { h.DeliveryId, h.ChangedAtUtc });

            builder.Entity<DeliveryStatusHistory>()
                .ToTable(t => t.HasCheckConstraint("CK_DeliveryStatusHistory_FromStatus", DataConstants.DeliveryStatusHistory.FromStatusCheckSql));

            builder.Entity<DeliveryStatusHistory>()
                .ToTable(t => t.HasCheckConstraint("CK_DeliveryStatusHistory_ToStatus", DataConstants.DeliveryStatusHistory.ToStatusCheckSql));

            // Reviews, support, notifications and audit.
            builder.Entity<Review>()
                .HasOne(r => r.Order)
                .WithMany(o => o.Reviews)
                .HasForeignKey(r => r.OrderId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Review>()
                .HasIndex(r => r.OrderId)
                .IsUnique();

            builder.Entity<Review>()
                .ToTable(t => t.HasCheckConstraint("CK_Review_Ratings", DataConstants.Review.RatingsCheckSql));

            builder.Entity<Review>()
                .ToTable(t => t.HasCheckConstraint("CK_Review_ModerationStatus", DataConstants.Review.ModerationStatusCheckSql));

            builder.Entity<ReviewReply>()
                .HasOne(r => r.Review)
                .WithOne(r => r.Reply)
                .HasForeignKey<ReviewReply>(r => r.ReviewId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<ReviewReply>()
                .HasOne(r => r.AuthorUser)
                .WithMany(u => u.ReviewReplies)
                .HasForeignKey(r => r.AuthorUserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<ReviewReply>()
                .ToTable(t => t.HasCheckConstraint("CK_ReviewReply_ModerationStatus", DataConstants.ReviewReply.ModerationStatusCheckSql));

            builder.Entity<SupportTicket>()
                .HasOne(t => t.CreatedByUser)
                .WithMany(u => u.CreatedSupportTickets)
                .HasForeignKey(t => t.CreatedByUserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<SupportTicket>()
                .HasOne(t => t.Order)
                .WithMany(o => o.SupportTickets)
                .HasForeignKey(t => t.OrderId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<SupportTicket>()
                .HasOne(t => t.AssignedAdmin)
                .WithMany(u => u.AssignedSupportTickets)
                .HasForeignKey(t => t.AssignedAdminId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<SupportTicket>()
                .HasIndex(t => new { t.CreatedByUserId, t.CreatedAtUtc });

            builder.Entity<SupportTicket>()
                .HasIndex(t => new { t.Status, t.UpdatedAtUtc });

            builder.Entity<SupportTicket>()
                .ToTable(t => t.HasCheckConstraint("CK_SupportTicket_Category", DataConstants.SupportTicket.CategoryCheckSql));

            builder.Entity<SupportTicket>()
                .ToTable(t => t.HasCheckConstraint("CK_SupportTicket_Status", DataConstants.SupportTicket.StatusCheckSql));

            builder.Entity<SupportTicketMessage>()
                .HasOne(m => m.SupportTicket)
                .WithMany(t => t.Messages)
                .HasForeignKey(m => m.SupportTicketId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<SupportTicketMessage>()
                .HasOne(m => m.AuthorUser)
                .WithMany(u => u.SupportMessages)
                .HasForeignKey(m => m.AuthorUserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<SupportTicketMessage>()
                .HasIndex(m => new { m.SupportTicketId, m.CreatedAtUtc });

            builder.Entity<Notification>()
                .HasOne(n => n.RecipientUser)
                .WithMany(u => u.Notifications)
                .HasForeignKey(n => n.RecipientUserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Notification>()
                .HasOne(n => n.Order)
                .WithMany(u => u.Notifications)
                .HasForeignKey(n => n.OrderId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Notification>()
                .HasOne(n => n.SupportTicket)
                .WithMany(u => u.Notifications)
                .HasForeignKey(n => n.SupportTicketId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Notification>()
                .HasIndex(n => new { n.RecipientUserId, n.CreatedAtUtc });

            builder.Entity<Notification>()
                .HasIndex(n => new { n.RecipientUserId, n.CreatedAtUtc }, "UnreadNotifications")
                .HasDatabaseName("IX_Notification_Unread")
                .HasFilter(DataConstants.Notification.UnreadFilter);

            builder.Entity<Notification>()
                .ToTable(t => t.HasCheckConstraint("CK_Notification_Type", DataConstants.Notification.TypeCheckSql));

            builder.Entity<AuditLog>()
                .HasOne(a => a.ActorUser)
                .WithMany(u => u.AuditLogs)
                .HasForeignKey(a => a.ActorUserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<AuditLog>()
                .HasIndex(a => new { a.EntityType, a.EntityId, a.CreatedAtUtc });

            builder.Entity<AuditLog>()
                .HasIndex(a => new { a.ActorUserId, a.CreatedAtUtc });
        }
    }
}
