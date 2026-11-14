using DishDrop.Data.Entities;
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
        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}
