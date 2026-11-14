using DishDrop.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DishDrop.Data.Configurations;

public sealed class CourierDeliveryAreaConfiguration : IEntityTypeConfiguration<CourierDeliveryArea>
{
    public void Configure(EntityTypeBuilder<CourierDeliveryArea> b)
    {
        b.ToTable("CourierDeliveryArea");
        b.HasKey(x => new { x.CourierId, x.DeliveryAreaId }).IsClustered(false);
        b.Property(x => x.CourierId).HasMaxLength(450).IsRequired();
        b.HasOne(x => x.Courier).WithMany().HasForeignKey(x => x.CourierId).OnDelete(DeleteBehavior.NoAction);
        b.HasOne(x => x.DeliveryArea).WithMany().HasForeignKey(x => x.DeliveryAreaId).OnDelete(DeleteBehavior.NoAction);
    }
}
