using DishDrop.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DishDrop.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<City> Cities => Set<City>();
    public DbSet<DeliveryArea> DeliveryAreas => Set<DeliveryArea>();
    public DbSet<CustomerAddress> CustomerAddresses => Set<CustomerAddress>();
    public DbSet<Restaurant> Restaurants => Set<Restaurant>();
    public DbSet<RestaurantStaff> RestaurantStaff => Set<RestaurantStaff>();
    public DbSet<RestaurantWorkingPeriod> RestaurantWorkingPeriods => Set<RestaurantWorkingPeriod>();
    public DbSet<RestaurantScheduleException> RestaurantScheduleExceptions => Set<RestaurantScheduleException>();
    public DbSet<RestaurantScheduleExceptionPeriod> RestaurantScheduleExceptionPeriods => Set<RestaurantScheduleExceptionPeriod>();
    public DbSet<RestaurantDeliveryArea> RestaurantDeliveryAreas => Set<RestaurantDeliveryArea>();
    public DbSet<CuisineType> CuisineTypes => Set<CuisineType>();
    public DbSet<RestaurantCuisineType> RestaurantCuisineTypes => Set<RestaurantCuisineType>();
    public DbSet<CourierProfile> CourierProfiles => Set<CourierProfile>();
    public DbSet<CourierDeliveryArea> CourierDeliveryAreas => Set<CourierDeliveryArea>();
    public DbSet<RestaurantApprovalHistory> RestaurantApprovalHistories => Set<RestaurantApprovalHistory>();
    public DbSet<CourierApprovalHistory> CourierApprovalHistories => Set<CourierApprovalHistory>();
    public DbSet<Menu> Menus => Set<Menu>();
    public DbSet<MenuCategory> MenuCategories => Set<MenuCategory>();
    public DbSet<Dish> Dishes => Set<Dish>();
    public DbSet<DishVariant> DishVariants => Set<DishVariant>();
    public DbSet<DishOptionGroup> DishOptionGroups => Set<DishOptionGroup>();
    public DbSet<DishOption> DishOptions => Set<DishOption>();
    public DbSet<Allergen> Allergens => Set<Allergen>();
    public DbSet<DishAllergen> DishAllergens => Set<DishAllergen>();
    public DbSet<DishOptionAllergen> DishOptionAllergens => Set<DishOptionAllergen>();
    public DbSet<Cart> Carts => Set<Cart>();
    public DbSet<CartItem> CartItems => Set<CartItem>();
    public DbSet<CartItemOption> CartItemOptions => Set<CartItemOption>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderDeliveryAddress> OrderDeliveryAddresses => Set<OrderDeliveryAddress>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<OrderItemOption> OrderItemOptions => Set<OrderItemOption>();
    public DbSet<OrderStatusHistory> OrderStatusHistories => Set<OrderStatusHistory>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<Delivery> Deliveries => Set<Delivery>();
    public DbSet<DeliveryAssignmentHistory> DeliveryAssignmentHistories => Set<DeliveryAssignmentHistory>();
    public DbSet<DeliveryStatusHistory> DeliveryStatusHistories => Set<DeliveryStatusHistory>();
    public DbSet<Review> Reviews => Set<Review>();
    public DbSet<ReviewReply> ReviewReplies => Set<ReviewReply>();
    public DbSet<SupportTicket> SupportTickets => Set<SupportTicket>();
    public DbSet<SupportTicketMessage> SupportTicketMessages => Set<SupportTicketMessage>();
    public DbSet<Notification> Notifications => Set<Notification>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        ConfigureIdentity(builder);
        ConfigureGeographyAndRestaurants(builder);
        ConfigureMenu(builder);
        ConfigureCartsAndOrders(builder);
        ConfigurePaymentsAndDeliveries(builder);
        ConfigureReviewsAndSupport(builder);
    }

    // Identity: preserve the original key lengths, defaults and unique email index.
    private static void ConfigureIdentity(ModelBuilder builder)
    {
        builder.Entity<IdentityUserLogin<string>>(b =>
        {
            b.Property(x => x.LoginProvider).HasMaxLength(DataConstants.Identity.LoginProviderMaxLength);
            b.Property(x => x.ProviderKey).HasMaxLength(DataConstants.Identity.ProviderKeyMaxLength);
        });

        builder.Entity<IdentityUserToken<string>>(b =>
        {
            b.Property(x => x.LoginProvider).HasMaxLength(DataConstants.Identity.LoginProviderMaxLength);
            b.Property(x => x.Name).HasMaxLength(DataConstants.Identity.TokenNameMaxLength);
        });

        builder.Entity<ApplicationUser>(b =>
        {
            b.Property(x => x.CreatedAtUtc).HasDefaultValueSql("SYSUTCDATETIME()");
            b.Property(x => x.FirstName).HasDefaultValue("");
            b.Property(x => x.LastName).HasDefaultValue("");
            b.Property(x => x.IsActive).HasDefaultValue(true);
            b.HasIndex(x => x.NormalizedEmail).HasDatabaseName("EmailIndex").IsUnique().HasFilter(DataConstants.ApplicationUser.NonemptyEmailFilter);
        });
    }

    // Geography, customer addresses, restaurants, schedules and couriers.
    private static void ConfigureGeographyAndRestaurants(ModelBuilder builder)
    {
        builder.Entity<City>(b =>
        {
            b.HasIndex(x => x.NormalizedName).IsUnique();
        });

        builder.Entity<DeliveryArea>(b =>
        {
            b.HasOne(x => x.City).WithMany().HasForeignKey(x => x.CityId).OnDelete(DeleteBehavior.NoAction);
            b.HasIndex(x => new { x.CityId, x.NormalizedName }).IsUnique();
        });

        builder.Entity<CustomerAddress>(b =>
        {
            b.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.NoAction);
            b.HasOne(x => x.DeliveryArea).WithMany().HasForeignKey(x => x.DeliveryAreaId).OnDelete(DeleteBehavior.NoAction);
            b.HasIndex(x => x.UserId).IsUnique().HasFilter(DataConstants.CustomerAddress.DefaultAddressFilter);
        });

        builder.Entity<Restaurant>(b =>
        {
            b.HasOne(x => x.City).WithMany().HasForeignKey(x => x.CityId).OnDelete(DeleteBehavior.NoAction);
            b.HasIndex(x => x.Slug).IsUnique();
            b.ToTable(t => t.HasCheckConstraint("CK_Restaurant_MinimumOrder", DataConstants.Restaurant.MinimumOrderCheckSql));
            b.ToTable(t => t.HasCheckConstraint("CK_Restaurant_Preparation", DataConstants.Restaurant.PreparationCheckSql));
            b.ToTable(t => t.HasCheckConstraint("CK_Restaurant_ApprovalStatus", DataConstants.Restaurant.ApprovalStatusCheckSql));
        });

        builder.Entity<RestaurantStaff>(b =>
        {
            b.HasKey(x => new { x.RestaurantId, x.UserId }).IsClustered(false);
            b.HasOne(x => x.Restaurant).WithMany().HasForeignKey(x => x.RestaurantId).OnDelete(DeleteBehavior.NoAction);
            b.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.NoAction);
            b.ToTable(t => t.HasCheckConstraint("CK_RestaurantStaff_StaffRole", DataConstants.RestaurantStaff.StaffRoleCheckSql));
        });

        builder.Entity<RestaurantWorkingPeriod>(b =>
        {
            b.HasOne(x => x.Restaurant).WithMany().HasForeignKey(x => x.RestaurantId).OnDelete(DeleteBehavior.NoAction);
            b.HasIndex(x => new { x.RestaurantId, x.DayOfWeek });
            b.ToTable(t => t.HasCheckConstraint("CK_RestaurantWorkingPeriod_Period", DataConstants.RestaurantWorkingPeriod.PeriodCheckSql));
            b.ToTable(t => t.HasCheckConstraint("CK_RestaurantWorkingPeriod_DayOfWeek", DataConstants.RestaurantWorkingPeriod.DayOfWeekCheckSql));
        });

        builder.Entity<RestaurantScheduleException>(b =>
        {
            b.HasOne(x => x.Restaurant).WithMany().HasForeignKey(x => x.RestaurantId).OnDelete(DeleteBehavior.NoAction);
            b.HasIndex(x => new { x.RestaurantId, x.Date }).IsUnique();
        });

        builder.Entity<RestaurantScheduleExceptionPeriod>(b =>
        {
            b.HasOne(x => x.RestaurantScheduleException).WithMany().HasForeignKey(x => x.RestaurantScheduleExceptionId).OnDelete(DeleteBehavior.NoAction);
            b.ToTable(t => t.HasCheckConstraint("CK_RestaurantScheduleExceptionPeriod_Period", DataConstants.RestaurantScheduleExceptionPeriod.PeriodCheckSql));
        });

        builder.Entity<RestaurantDeliveryArea>(b =>
        {
            b.HasKey(x => new { x.RestaurantId, x.DeliveryAreaId });
            b.HasOne(x => x.Restaurant).WithMany().HasForeignKey(x => x.RestaurantId).OnDelete(DeleteBehavior.NoAction);
            b.HasOne(x => x.DeliveryArea).WithMany().HasForeignKey(x => x.DeliveryAreaId).OnDelete(DeleteBehavior.NoAction);
            b.ToTable(t => t.HasCheckConstraint("CK_RestaurantDeliveryArea_Fee", DataConstants.RestaurantDeliveryArea.FeeCheckSql));
        });

        builder.Entity<CuisineType>(b =>
        {
            b.HasIndex(x => x.NormalizedName).IsUnique();
        });

        builder.Entity<RestaurantCuisineType>(b =>
        {
            b.HasKey(x => new { x.RestaurantId, x.CuisineTypeId });
            b.HasOne(x => x.Restaurant).WithMany().HasForeignKey(x => x.RestaurantId).OnDelete(DeleteBehavior.NoAction);
            b.HasOne(x => x.CuisineType).WithMany().HasForeignKey(x => x.CuisineTypeId).OnDelete(DeleteBehavior.NoAction);
        });

        builder.Entity<CourierProfile>(b =>
        {
            b.HasOne(x => x.User).WithOne().HasForeignKey<CourierProfile>(x => x.UserId).OnDelete(DeleteBehavior.NoAction);
            b.ToTable(t => t.HasCheckConstraint("CK_CourierProfile_ApprovalStatus", DataConstants.CourierProfile.ApprovalStatusCheckSql));
            b.ToTable(t => t.HasCheckConstraint("CK_CourierProfile_VehicleType", DataConstants.CourierProfile.VehicleTypeCheckSql));
        });

        builder.Entity<CourierDeliveryArea>(b =>
        {
            b.HasKey(x => new { x.CourierId, x.DeliveryAreaId }).IsClustered(false);
            b.HasOne(x => x.Courier).WithMany().HasForeignKey(x => x.CourierId).OnDelete(DeleteBehavior.NoAction);
            b.HasOne(x => x.DeliveryArea).WithMany().HasForeignKey(x => x.DeliveryAreaId).OnDelete(DeleteBehavior.NoAction);
        });

        builder.Entity<RestaurantApprovalHistory>(b =>
        {
            b.HasOne(x => x.Restaurant).WithMany().HasForeignKey(x => x.RestaurantId).OnDelete(DeleteBehavior.NoAction);
            b.HasOne(x => x.ChangedByUser).WithMany().HasForeignKey(x => x.ChangedByUserId).OnDelete(DeleteBehavior.NoAction);
            b.HasIndex(x => new { x.RestaurantId, x.ChangedAtUtc });
            b.ToTable(t => t.HasCheckConstraint("CK_RestaurantApprovalHistory_FromStatus", DataConstants.RestaurantApprovalHistory.FromStatusCheckSql));
            b.ToTable(t => t.HasCheckConstraint("CK_RestaurantApprovalHistory_ToStatus", DataConstants.RestaurantApprovalHistory.ToStatusCheckSql));
        });

        builder.Entity<CourierApprovalHistory>(b =>
        {
            b.HasOne(x => x.Courier).WithMany().HasForeignKey(x => x.CourierId).OnDelete(DeleteBehavior.NoAction);
            b.HasOne(x => x.ChangedByUser).WithMany().HasForeignKey(x => x.ChangedByUserId).OnDelete(DeleteBehavior.NoAction);
            b.HasIndex(x => new { x.CourierId, x.ChangedAtUtc });
            b.ToTable(t => t.HasCheckConstraint("CK_CourierApprovalHistory_FromStatus", DataConstants.CourierApprovalHistory.FromStatusCheckSql));
            b.ToTable(t => t.HasCheckConstraint("CK_CourierApprovalHistory_ToStatus", DataConstants.CourierApprovalHistory.ToStatusCheckSql));
        });
    }

    // Menus, variants, options and allergen links.
    private static void ConfigureMenu(ModelBuilder builder)
    {
        builder.Entity<Menu>(b =>
        {
            b.HasOne(x => x.Restaurant).WithMany().HasForeignKey(x => x.RestaurantId).OnDelete(DeleteBehavior.NoAction);
            b.HasIndex(x => new { x.RestaurantId, x.DisplayOrder });
        });

        builder.Entity<MenuCategory>(b =>
        {
            b.HasOne(x => x.Menu).WithMany().HasForeignKey(x => x.MenuId).OnDelete(DeleteBehavior.NoAction);
            b.HasIndex(x => new { x.MenuId, x.NormalizedName }).IsUnique();
        });

        builder.Entity<Dish>(b =>
        {
            b.HasOne(x => x.MenuCategory).WithMany().HasForeignKey(x => x.MenuCategoryId).OnDelete(DeleteBehavior.NoAction);
            b.HasIndex(x => new { x.MenuCategoryId, x.DisplayOrder });
        });

        builder.Entity<DishVariant>(b =>
        {
            b.HasOne(x => x.Dish).WithMany().HasForeignKey(x => x.DishId).OnDelete(DeleteBehavior.NoAction);
            b.HasIndex(x => new { x.DishId, x.NormalizedName }).IsUnique();
            b.ToTable(t => t.HasCheckConstraint("CK_DishVariant_Price", DataConstants.DishVariant.PriceCheckSql));
            b.ToTable(t => t.HasCheckConstraint("CK_DishVariant_Portion", DataConstants.DishVariant.PortionCheckSql));
            b.ToTable(t => t.HasCheckConstraint("CK_DishVariant_PortionUnit", DataConstants.DishVariant.PortionUnitCheckSql));
        });

        builder.Entity<DishOptionGroup>(b =>
        {
            b.HasOne(x => x.Dish).WithMany().HasForeignKey(x => x.DishId).OnDelete(DeleteBehavior.NoAction);
            b.ToTable(t => t.HasCheckConstraint("CK_DishOptionGroup_Selections", DataConstants.DishOptionGroup.SelectionsCheckSql));
        });

        builder.Entity<DishOption>(b =>
        {
            b.HasOne(x => x.DishOptionGroup).WithMany().HasForeignKey(x => x.DishOptionGroupId).OnDelete(DeleteBehavior.NoAction);
            b.ToTable(t => t.HasCheckConstraint("CK_DishOption_Price", DataConstants.DishOption.PriceCheckSql));
        });

        builder.Entity<Allergen>(b =>
        {
            b.HasIndex(x => x.Code).IsUnique();
        });

        builder.Entity<DishAllergen>(b =>
        {
            b.HasKey(x => new { x.DishId, x.AllergenId });
            b.HasOne(x => x.Dish).WithMany().HasForeignKey(x => x.DishId).OnDelete(DeleteBehavior.NoAction);
            b.HasOne(x => x.Allergen).WithMany().HasForeignKey(x => x.AllergenId).OnDelete(DeleteBehavior.NoAction);
        });

        builder.Entity<DishOptionAllergen>(b =>
        {
            b.HasKey(x => new { x.DishOptionId, x.AllergenId });
            b.HasOne(x => x.DishOption).WithMany().HasForeignKey(x => x.DishOptionId).OnDelete(DeleteBehavior.NoAction);
            b.HasOne(x => x.Allergen).WithMany().HasForeignKey(x => x.AllergenId).OnDelete(DeleteBehavior.NoAction);
        });
    }

    // Carts, immutable purchase snapshots, order amounts and histories.
    private static void ConfigureCartsAndOrders(ModelBuilder builder)
    {
        builder.Entity<Cart>(b =>
        {
            b.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.NoAction);
            b.HasOne(x => x.Restaurant).WithMany().HasForeignKey(x => x.RestaurantId).OnDelete(DeleteBehavior.NoAction);
            b.HasIndex(x => x.UserId).IsUnique();
        });

        builder.Entity<CartItem>(b =>
        {
            b.HasOne(x => x.Cart).WithMany().HasForeignKey(x => x.CartId).OnDelete(DeleteBehavior.NoAction);
            b.HasOne(x => x.DishVariant).WithMany().HasForeignKey(x => x.DishVariantId).OnDelete(DeleteBehavior.NoAction);
            b.ToTable(t => t.HasCheckConstraint("CK_CartItem_Quantity", DataConstants.CartItem.QuantityCheckSql));
        });

        builder.Entity<CartItemOption>(b =>
        {
            b.HasKey(x => new { x.CartItemId, x.DishOptionId });
            b.HasOne(x => x.CartItem).WithMany().HasForeignKey(x => x.CartItemId).OnDelete(DeleteBehavior.NoAction);
            b.HasOne(x => x.DishOption).WithMany().HasForeignKey(x => x.DishOptionId).OnDelete(DeleteBehavior.NoAction);
        });

        builder.Entity<Order>(b =>
        {
            b.HasOne(x => x.Customer).WithMany().HasForeignKey(x => x.CustomerId).OnDelete(DeleteBehavior.NoAction);
            b.HasOne(x => x.Restaurant).WithMany().HasForeignKey(x => x.RestaurantId).OnDelete(DeleteBehavior.NoAction);
            b.HasIndex(x => x.OrderNumber).IsUnique();
            b.HasIndex(x => x.CheckoutToken).IsUnique();
            b.HasIndex(x => new { x.CustomerId, x.CreatedAtUtc });
            b.HasIndex(x => new { x.RestaurantId, x.Status, x.CreatedAtUtc });
            b.ToTable(t => t.HasCheckConstraint("CK_Order_Amounts", DataConstants.Order.AmountsCheckSql));
            b.ToTable(t => t.HasCheckConstraint("CK_Order_Currency", DataConstants.Order.CurrencyCheckSql));
            b.ToTable(t => t.HasCheckConstraint("CK_Order_Hash", DataConstants.Order.HashCheckSql));
            b.ToTable(t => t.HasCheckConstraint("CK_Order_Status", DataConstants.Order.StatusCheckSql));
        });

        builder.Entity<OrderDeliveryAddress>(b =>
        {
            b.HasOne(x => x.Order).WithOne().HasForeignKey<OrderDeliveryAddress>(x => x.OrderId).OnDelete(DeleteBehavior.NoAction);
            b.HasOne(x => x.DeliveryArea).WithMany().HasForeignKey(x => x.DeliveryAreaId).OnDelete(DeleteBehavior.NoAction);
        });

        builder.Entity<OrderItem>(b =>
        {
            b.HasOne(x => x.Order).WithMany().HasForeignKey(x => x.OrderId).OnDelete(DeleteBehavior.NoAction);
            b.HasOne(x => x.DishVariant).WithMany().HasForeignKey(x => x.DishVariantId).OnDelete(DeleteBehavior.NoAction);
            b.ToTable(t => t.HasCheckConstraint("CK_OrderItem_Amounts", DataConstants.OrderItem.AmountsCheckSql));
            b.ToTable(t => t.HasCheckConstraint("CK_OrderItem_Quantity", DataConstants.OrderItem.QuantityCheckSql));
        });

        builder.Entity<OrderItemOption>(b =>
        {
            b.HasOne(x => x.OrderItem).WithMany().HasForeignKey(x => x.OrderItemId).OnDelete(DeleteBehavior.NoAction);
            b.HasOne(x => x.DishOption).WithMany().HasForeignKey(x => x.DishOptionId).OnDelete(DeleteBehavior.NoAction);
            b.HasIndex(x => new { x.OrderItemId, x.DishOptionId }).IsUnique();
            b.ToTable(t => t.HasCheckConstraint("CK_OrderItemOption_Price", DataConstants.OrderItemOption.PriceCheckSql));
        });

        builder.Entity<OrderStatusHistory>(b =>
        {
            b.HasOne(x => x.Order).WithMany().HasForeignKey(x => x.OrderId).OnDelete(DeleteBehavior.NoAction);
            b.HasOne(x => x.ChangedByUser).WithMany().HasForeignKey(x => x.ChangedByUserId).OnDelete(DeleteBehavior.NoAction);
            b.HasIndex(x => new { x.OrderId, x.ChangedAtUtc });
            b.ToTable(t => t.HasCheckConstraint("CK_OrderStatusHistory_FromStatus", DataConstants.OrderStatusHistory.FromStatusCheckSql));
            b.ToTable(t => t.HasCheckConstraint("CK_OrderStatusHistory_ToStatus", DataConstants.OrderStatusHistory.ToStatusCheckSql));
        });
    }

    // Cash payments, deliveries and assignment/status histories.
    private static void ConfigurePaymentsAndDeliveries(ModelBuilder builder)
    {
        builder.Entity<Payment>(b =>
        {
            b.HasOne(x => x.Order).WithMany().HasForeignKey(x => x.OrderId).OnDelete(DeleteBehavior.NoAction);
            b.HasOne(x => x.CollectedByUser).WithMany().HasForeignKey(x => x.CollectedByUserId).OnDelete(DeleteBehavior.NoAction);
            b.HasIndex(x => x.OrderId).IsUnique();
            b.ToTable(t => t.HasCheckConstraint("CK_Payment_Amount", DataConstants.Payment.AmountCheckSql));
            b.ToTable(t => t.HasCheckConstraint("CK_Payment_Currency", DataConstants.Payment.CurrencyCheckSql));
            b.ToTable(t => t.HasCheckConstraint("CK_Payment_Method", DataConstants.Payment.MethodCheckSql));
            b.ToTable(t => t.HasCheckConstraint("CK_Payment_Status", DataConstants.Payment.StatusCheckSql));
        });

        builder.Entity<Delivery>(b =>
        {
            b.HasOne(x => x.Order).WithMany().HasForeignKey(x => x.OrderId).OnDelete(DeleteBehavior.NoAction);
            b.HasOne(x => x.Courier).WithMany().HasForeignKey(x => x.CourierId).OnDelete(DeleteBehavior.NoAction);
            b.HasOne(x => x.DeliveryArea).WithMany().HasForeignKey(x => x.DeliveryAreaId).OnDelete(DeleteBehavior.NoAction);
            b.HasIndex(x => x.OrderId).IsUnique();
            b.HasIndex(x => x.CourierId).IsUnique().HasFilter(DataConstants.Delivery.ActiveCourierFilter);
            b.ToTable(t => t.HasCheckConstraint("CK_Delivery_Courier", DataConstants.Delivery.CourierCheckSql));
            b.ToTable(t => t.HasCheckConstraint("CK_Delivery_Status", DataConstants.Delivery.StatusCheckSql));
        });

        builder.Entity<DeliveryAssignmentHistory>(b =>
        {
            b.HasOne(x => x.Delivery).WithMany().HasForeignKey(x => x.DeliveryId).OnDelete(DeleteBehavior.NoAction);
            b.HasOne(x => x.Courier).WithMany().HasForeignKey(x => x.CourierId).OnDelete(DeleteBehavior.NoAction);
            b.HasIndex(x => x.DeliveryId).IsUnique().HasFilter(DataConstants.DeliveryAssignmentHistory.OpenAssignmentFilter);
            b.ToTable(t => t.HasCheckConstraint("CK_DeliveryAssignmentHistory_Release", DataConstants.DeliveryAssignmentHistory.ReleaseCheckSql));
        });

        builder.Entity<DeliveryStatusHistory>(b =>
        {
            b.HasOne(x => x.Delivery).WithMany().HasForeignKey(x => x.DeliveryId).OnDelete(DeleteBehavior.NoAction);
            b.HasOne(x => x.ChangedByUser).WithMany().HasForeignKey(x => x.ChangedByUserId).OnDelete(DeleteBehavior.NoAction);
            b.HasIndex(x => new { x.DeliveryId, x.ChangedAtUtc });
            b.ToTable(t => t.HasCheckConstraint("CK_DeliveryStatusHistory_FromStatus", DataConstants.DeliveryStatusHistory.FromStatusCheckSql));
            b.ToTable(t => t.HasCheckConstraint("CK_DeliveryStatusHistory_ToStatus", DataConstants.DeliveryStatusHistory.ToStatusCheckSql));
        });
    }

    // Reviews, support, notifications and audit.
    private static void ConfigureReviewsAndSupport(ModelBuilder builder)
    {
        builder.Entity<Review>(b =>
        {
            b.HasOne(x => x.Order).WithMany().HasForeignKey(x => x.OrderId).OnDelete(DeleteBehavior.NoAction);
            b.HasIndex(x => x.OrderId).IsUnique();
            b.ToTable(t => t.HasCheckConstraint("CK_Review_Ratings", DataConstants.Review.RatingsCheckSql));
            b.ToTable(t => t.HasCheckConstraint("CK_Review_ModerationStatus", DataConstants.Review.ModerationStatusCheckSql));
        });

        builder.Entity<ReviewReply>(b =>
        {
            b.HasOne(x => x.Review).WithOne().HasForeignKey<ReviewReply>(x => x.ReviewId).OnDelete(DeleteBehavior.NoAction);
            b.HasOne(x => x.AuthorUser).WithMany().HasForeignKey(x => x.AuthorUserId).OnDelete(DeleteBehavior.NoAction);
            b.ToTable(t => t.HasCheckConstraint("CK_ReviewReply_ModerationStatus", DataConstants.ReviewReply.ModerationStatusCheckSql));
        });

        builder.Entity<SupportTicket>(b =>
        {
            b.HasOne(x => x.CreatedByUser).WithMany().HasForeignKey(x => x.CreatedByUserId).OnDelete(DeleteBehavior.NoAction);
            b.HasOne(x => x.Order).WithMany().HasForeignKey(x => x.OrderId).OnDelete(DeleteBehavior.NoAction);
            b.HasOne(x => x.AssignedAdmin).WithMany().HasForeignKey(x => x.AssignedAdminId).OnDelete(DeleteBehavior.NoAction);
            b.HasIndex(x => new { x.CreatedByUserId, x.CreatedAtUtc });
            b.HasIndex(x => new { x.Status, x.UpdatedAtUtc });
            b.ToTable(t => t.HasCheckConstraint("CK_SupportTicket_Category", DataConstants.SupportTicket.CategoryCheckSql));
            b.ToTable(t => t.HasCheckConstraint("CK_SupportTicket_Status", DataConstants.SupportTicket.StatusCheckSql));
        });

        builder.Entity<SupportTicketMessage>(b =>
        {
            b.HasOne(x => x.SupportTicket).WithMany().HasForeignKey(x => x.SupportTicketId).OnDelete(DeleteBehavior.NoAction);
            b.HasOne(x => x.AuthorUser).WithMany().HasForeignKey(x => x.AuthorUserId).OnDelete(DeleteBehavior.NoAction);
            b.HasIndex(x => new { x.SupportTicketId, x.CreatedAtUtc });
        });

        builder.Entity<Notification>(b =>
        {
            b.HasOne(x => x.RecipientUser).WithMany().HasForeignKey(x => x.RecipientUserId).OnDelete(DeleteBehavior.NoAction);
            b.HasOne(x => x.Order).WithMany().HasForeignKey(x => x.OrderId).OnDelete(DeleteBehavior.NoAction);
            b.HasOne(x => x.SupportTicket).WithMany().HasForeignKey(x => x.SupportTicketId).OnDelete(DeleteBehavior.NoAction);
            b.HasIndex(x => new { x.RecipientUserId, x.CreatedAtUtc });
            b.HasIndex(x => new { x.RecipientUserId, x.CreatedAtUtc }, "UnreadNotifications").HasDatabaseName("IX_Notification_Unread").HasFilter(DataConstants.Notification.UnreadFilter);
            b.ToTable(t => t.HasCheckConstraint("CK_Notification_Type", DataConstants.Notification.TypeCheckSql));
        });

        builder.Entity<AuditLog>(b =>
        {
            b.HasOne(x => x.ActorUser).WithMany().HasForeignKey(x => x.ActorUserId).OnDelete(DeleteBehavior.NoAction);
            b.HasIndex(x => new { x.EntityType, x.EntityId, x.CreatedAtUtc });
            b.HasIndex(x => new { x.ActorUserId, x.CreatedAtUtc });
        });
    }
}
