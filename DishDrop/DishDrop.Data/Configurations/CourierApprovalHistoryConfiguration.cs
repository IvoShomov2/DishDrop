using DishDrop.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DishDrop.Data.Configurations;

public sealed class CourierApprovalHistoryConfiguration : IEntityTypeConfiguration<CourierApprovalHistory>
{
    public void Configure(EntityTypeBuilder<CourierApprovalHistory> b)
    {
        b.ToTable("CourierApprovalHistory");
        b.HasKey(x => x.Id);
        b.Property(x => x.CourierId).HasMaxLength(450).IsRequired();
        b.Property(x => x.ChangedByUserId).HasMaxLength(450).IsRequired();
        b.Property(x => x.Reason).HasMaxLength(1000).IsRequired();
        b.HasOne(x => x.Courier).WithMany().HasForeignKey(x => x.CourierId).OnDelete(DeleteBehavior.NoAction);
        b.HasOne(x => x.ChangedByUser).WithMany().HasForeignKey(x => x.ChangedByUserId).OnDelete(DeleteBehavior.NoAction);
        b.HasIndex(x => new { x.CourierId, x.ChangedAtUtc });
        b.ToTable(t => t.HasCheckConstraint("CK_CourierApprovalHistory_FromStatus", "[FromStatus] IS NULL OR [FromStatus] BETWEEN 0 AND 2"));
        b.ToTable(t => t.HasCheckConstraint("CK_CourierApprovalHistory_ToStatus", "[ToStatus] BETWEEN 0 AND 2"));
    }
}
