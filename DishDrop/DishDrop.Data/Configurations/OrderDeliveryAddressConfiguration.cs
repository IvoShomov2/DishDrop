using DishDrop.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DishDrop.Data.Configurations;

public sealed class OrderDeliveryAddressConfiguration : IEntityTypeConfiguration<OrderDeliveryAddress>
{
    public void Configure(EntityTypeBuilder<OrderDeliveryAddress> b)
    {
        b.ToTable("OrderDeliveryAddress");
        b.HasKey(x => x.OrderId);
        b.Property(x => x.RecipientName).HasMaxLength(200).IsRequired();
        b.Property(x => x.PhoneNumber).HasMaxLength(30).IsRequired();
        b.Property(x => x.CityName).HasMaxLength(100).IsRequired();
        b.Property(x => x.AreaName).HasMaxLength(100).IsRequired();
        b.Property(x => x.StreetAddress).HasMaxLength(300).IsRequired();
        b.Property(x => x.Building).HasMaxLength(30);
        b.Property(x => x.Entrance).HasMaxLength(30);
        b.Property(x => x.Floor).HasMaxLength(30);
        b.Property(x => x.Apartment).HasMaxLength(30);
        b.Property(x => x.DeliveryInstructions).HasMaxLength(1000);
        b.HasOne(x => x.Order).WithOne().HasForeignKey<OrderDeliveryAddress>(x => x.OrderId).OnDelete(DeleteBehavior.NoAction);
        b.HasOne(x => x.DeliveryArea).WithMany().HasForeignKey(x => x.DeliveryAreaId).OnDelete(DeleteBehavior.NoAction);
    }
}
