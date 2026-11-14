using DishDrop.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DishDrop.Data.Configurations;

public sealed class DeliveryConfiguration : IEntityTypeConfiguration<Delivery>
{
    public void Configure(EntityTypeBuilder<Delivery> b)
    {
        b.ToTable("Delivery");
        b.HasKey(x => x.Id);
        b.Property(x => x.CourierId).HasMaxLength(450);
        b.Property(x => x.RowVersion).IsRowVersion();
        b.HasOne(x => x.Order).WithMany().HasForeignKey(x => x.OrderId).OnDelete(DeleteBehavior.NoAction);
        b.HasOne(x => x.Courier).WithMany().HasForeignKey(x => x.CourierId).OnDelete(DeleteBehavior.NoAction);
        b.HasOne(x => x.DeliveryArea).WithMany().HasForeignKey(x => x.DeliveryAreaId).OnDelete(DeleteBehavior.NoAction);
        b.HasIndex(x => x.OrderId).IsUnique();
        b.HasIndex(x => x.CourierId).IsUnique().HasFilter("[Status] IN (1, 2)");
        b.ToTable(t => t.HasCheckConstraint("CK_Delivery_Courier", "([Status] = 0 AND [CourierId] IS NULL) OR ([Status] IN (1, 2, 3, 4) AND [CourierId] IS NOT NULL) OR [Status] = 5"));
        b.ToTable(t => t.HasCheckConstraint("CK_Delivery_Status", "[Status] BETWEEN 0 AND 5"));
    }
}
