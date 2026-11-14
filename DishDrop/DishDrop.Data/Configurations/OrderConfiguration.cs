using DishDrop.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DishDrop.Data.Configurations;

public sealed class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> b)
    {
        b.ToTable("Order");
        b.HasKey(x => x.Id);
        b.Property(x => x.OrderNumber).HasMaxLength(40).IsRequired();
        b.Property(x => x.CustomerId).HasMaxLength(450).IsRequired();
        b.Property(x => x.Subtotal).HasPrecision(18, 2);
        b.Property(x => x.DeliveryFee).HasPrecision(18, 2);
        b.Property(x => x.TotalAmount).HasPrecision(18, 2);
        b.Property(x => x.CurrencyCode).HasMaxLength(3).IsRequired();
        b.Property(x => x.CustomerNote).HasMaxLength(1000);
        b.Property(x => x.CheckoutRequestHash).HasMaxLength(64).IsRequired();
        b.Property(x => x.RestaurantNameSnapshot).HasMaxLength(150).IsRequired();
        b.Property(x => x.PickupCitySnapshot).HasMaxLength(100).IsRequired();
        b.Property(x => x.PickupAddressSnapshot).HasMaxLength(300).IsRequired();
        b.Property(x => x.PickupPhoneSnapshot).HasMaxLength(30).IsRequired();
        b.Property(x => x.RowVersion).IsRowVersion();
        b.HasOne(x => x.Customer).WithMany().HasForeignKey(x => x.CustomerId).OnDelete(DeleteBehavior.NoAction);
        b.HasOne(x => x.Restaurant).WithMany().HasForeignKey(x => x.RestaurantId).OnDelete(DeleteBehavior.NoAction);
        b.HasIndex(x => x.OrderNumber).IsUnique();
        b.HasIndex(x => x.CheckoutToken).IsUnique();
        b.HasIndex(x => new { x.CustomerId, x.CreatedAtUtc });
        b.HasIndex(x => new { x.RestaurantId, x.Status, x.CreatedAtUtc });
        b.ToTable(t => t.HasCheckConstraint("CK_Order_Amounts", "[Subtotal] >= 0 AND [DeliveryFee] >= 0 AND [TotalAmount] = [Subtotal] + [DeliveryFee]"));
        b.ToTable(t => t.HasCheckConstraint("CK_Order_Currency", "LEN([CurrencyCode]) = 3 AND [CurrencyCode] COLLATE Latin1_General_100_BIN2 NOT LIKE '%[^A-Z]%'"));
        b.ToTable(t => t.HasCheckConstraint("CK_Order_Hash", "LEN([CheckoutRequestHash]) = 64 AND [CheckoutRequestHash] COLLATE Latin1_General_100_BIN2 NOT LIKE '%[^0-9A-Fa-f]%'"));
        b.ToTable(t => t.HasCheckConstraint("CK_Order_Status", "[Status] BETWEEN 0 AND 8"));
    }
}
