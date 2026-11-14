using DishDrop.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DishDrop.Data.Configurations;

public sealed class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> b)
    {
        b.ToTable("Review");
        b.HasKey(x => x.Id);
        b.Property(x => x.Comment).HasMaxLength(2000);
        b.Property(x => x.RowVersion).IsRowVersion();
        b.HasOne(x => x.Order).WithMany().HasForeignKey(x => x.OrderId).OnDelete(DeleteBehavior.NoAction);
        b.HasIndex(x => x.OrderId).IsUnique();
        b.ToTable(t => t.HasCheckConstraint("CK_Review_Ratings", "[RestaurantRating] BETWEEN 1 AND 5 AND ([DeliveryRating] IS NULL OR [DeliveryRating] BETWEEN 1 AND 5)"));
        b.ToTable(t => t.HasCheckConstraint("CK_Review_ModerationStatus", "[ModerationStatus] BETWEEN 0 AND 1"));
    }
}
