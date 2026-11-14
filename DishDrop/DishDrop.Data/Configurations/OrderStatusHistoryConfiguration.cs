using DishDrop.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DishDrop.Data.Configurations;

public sealed class OrderStatusHistoryConfiguration : IEntityTypeConfiguration<OrderStatusHistory>
{
    public void Configure(EntityTypeBuilder<OrderStatusHistory> b)
    {
        b.ToTable("OrderStatusHistory");
        b.HasKey(x => x.Id);
        b.Property(x => x.ChangedByUserId).HasMaxLength(450);
        b.Property(x => x.Reason).HasMaxLength(1000).IsRequired();
        b.HasOne(x => x.Order).WithMany().HasForeignKey(x => x.OrderId).OnDelete(DeleteBehavior.NoAction);
        b.HasOne(x => x.ChangedByUser).WithMany().HasForeignKey(x => x.ChangedByUserId).OnDelete(DeleteBehavior.NoAction);
        b.HasIndex(x => new { x.OrderId, x.ChangedAtUtc });
        b.ToTable(t => t.HasCheckConstraint("CK_OrderStatusHistory_FromStatus", "[FromStatus] IS NULL OR [FromStatus] BETWEEN 0 AND 8"));
        b.ToTable(t => t.HasCheckConstraint("CK_OrderStatusHistory_ToStatus", "[ToStatus] BETWEEN 0 AND 8"));
    }
}
