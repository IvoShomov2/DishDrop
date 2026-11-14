using DishDrop.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DishDrop.Data.Configurations;

public sealed class RestaurantApprovalHistoryConfiguration : IEntityTypeConfiguration<RestaurantApprovalHistory>
{
    public void Configure(EntityTypeBuilder<RestaurantApprovalHistory> b)
    {
        b.ToTable("RestaurantApprovalHistory");
        b.HasKey(x => x.Id);
        b.Property(x => x.ChangedByUserId).HasMaxLength(450).IsRequired();
        b.Property(x => x.Reason).HasMaxLength(1000).IsRequired();
        b.HasOne(x => x.Restaurant).WithMany().HasForeignKey(x => x.RestaurantId).OnDelete(DeleteBehavior.NoAction);
        b.HasOne(x => x.ChangedByUser).WithMany().HasForeignKey(x => x.ChangedByUserId).OnDelete(DeleteBehavior.NoAction);
        b.HasIndex(x => new { x.RestaurantId, x.ChangedAtUtc });
        b.ToTable(t => t.HasCheckConstraint("CK_RestaurantApprovalHistory_FromStatus", "[FromStatus] IS NULL OR [FromStatus] BETWEEN 0 AND 2"));
        b.ToTable(t => t.HasCheckConstraint("CK_RestaurantApprovalHistory_ToStatus", "[ToStatus] BETWEEN 0 AND 2"));
    }
}
