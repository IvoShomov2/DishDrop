using DishDrop.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DishDrop.Data.Configurations;

public sealed class DeliveryStatusHistoryConfiguration : IEntityTypeConfiguration<DeliveryStatusHistory>
{
    public void Configure(EntityTypeBuilder<DeliveryStatusHistory> b)
    {
        b.ToTable("DeliveryStatusHistory");
        b.HasKey(x => x.Id);
        b.Property(x => x.ChangedByUserId).HasMaxLength(450);
        b.Property(x => x.Reason).HasMaxLength(1000).IsRequired();
        b.HasOne(x => x.Delivery).WithMany().HasForeignKey(x => x.DeliveryId).OnDelete(DeleteBehavior.NoAction);
        b.HasOne(x => x.ChangedByUser).WithMany().HasForeignKey(x => x.ChangedByUserId).OnDelete(DeleteBehavior.NoAction);
        b.HasIndex(x => new { x.DeliveryId, x.ChangedAtUtc });
        b.ToTable(t => t.HasCheckConstraint("CK_DeliveryStatusHistory_FromStatus", "[FromStatus] IS NULL OR [FromStatus] BETWEEN 0 AND 5"));
        b.ToTable(t => t.HasCheckConstraint("CK_DeliveryStatusHistory_ToStatus", "[ToStatus] BETWEEN 0 AND 5"));
    }
}
