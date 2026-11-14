using DishDrop.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DishDrop.Data.Configurations;

public sealed class DeliveryAssignmentHistoryConfiguration : IEntityTypeConfiguration<DeliveryAssignmentHistory>
{
    public void Configure(EntityTypeBuilder<DeliveryAssignmentHistory> b)
    {
        b.ToTable("DeliveryAssignmentHistory");
        b.HasKey(x => x.Id);
        b.Property(x => x.CourierId).HasMaxLength(450).IsRequired();
        b.Property(x => x.ReleaseReason).HasMaxLength(1000);
        b.HasOne(x => x.Delivery).WithMany().HasForeignKey(x => x.DeliveryId).OnDelete(DeleteBehavior.NoAction);
        b.HasOne(x => x.Courier).WithMany().HasForeignKey(x => x.CourierId).OnDelete(DeleteBehavior.NoAction);
        b.HasIndex(x => x.DeliveryId).IsUnique().HasFilter("[ReleasedAtUtc] IS NULL");
        b.ToTable(t => t.HasCheckConstraint("CK_DeliveryAssignmentHistory_Release", "[ReleasedAtUtc] IS NULL OR [ReleasedAtUtc] >= [AssignedAtUtc]"));
    }
}
