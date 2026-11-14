using DishDrop.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DishDrop.Data.Configurations;

public sealed class ReviewReplyConfiguration : IEntityTypeConfiguration<ReviewReply>
{
    public void Configure(EntityTypeBuilder<ReviewReply> b)
    {
        b.ToTable("ReviewReply");
        b.HasKey(x => x.ReviewId);
        b.Property(x => x.AuthorUserId).HasMaxLength(450).IsRequired();
        b.Property(x => x.Text).HasMaxLength(2000).IsRequired();
        b.HasOne(x => x.Review).WithOne().HasForeignKey<ReviewReply>(x => x.ReviewId).OnDelete(DeleteBehavior.NoAction);
        b.HasOne(x => x.AuthorUser).WithMany().HasForeignKey(x => x.AuthorUserId).OnDelete(DeleteBehavior.NoAction);
        b.ToTable(t => t.HasCheckConstraint("CK_ReviewReply_ModerationStatus", "[ModerationStatus] BETWEEN 0 AND 1"));
    }
}
