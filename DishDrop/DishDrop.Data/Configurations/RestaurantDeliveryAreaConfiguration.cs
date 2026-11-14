using DishDrop.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DishDrop.Data.Configurations;

public sealed class RestaurantDeliveryAreaConfiguration : IEntityTypeConfiguration<RestaurantDeliveryArea>
{
    public void Configure(EntityTypeBuilder<RestaurantDeliveryArea> b)
    {
        b.ToTable("RestaurantDeliveryArea");
        b.HasKey(x => new { x.RestaurantId, x.DeliveryAreaId });
        b.Property(x => x.DeliveryFee).HasPrecision(18, 2);
        b.HasOne(x => x.Restaurant).WithMany().HasForeignKey(x => x.RestaurantId).OnDelete(DeleteBehavior.NoAction);
        b.HasOne(x => x.DeliveryArea).WithMany().HasForeignKey(x => x.DeliveryAreaId).OnDelete(DeleteBehavior.NoAction);
        b.ToTable(t => t.HasCheckConstraint("CK_RestaurantDeliveryArea_Fee", "[DeliveryFee] >= 0"));
    }
}
