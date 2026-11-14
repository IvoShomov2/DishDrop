using DishDrop.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DishDrop.Data.Configurations;

public sealed class RestaurantStaffConfiguration : IEntityTypeConfiguration<RestaurantStaff>
{
    public void Configure(EntityTypeBuilder<RestaurantStaff> b)
    {
        b.ToTable("RestaurantStaff");
        b.HasKey(x => new { x.RestaurantId, x.UserId }).IsClustered(false);
        b.Property(x => x.UserId).HasMaxLength(450).IsRequired();
        b.HasOne(x => x.Restaurant).WithMany().HasForeignKey(x => x.RestaurantId).OnDelete(DeleteBehavior.NoAction);
        b.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.NoAction);
        b.ToTable(t => t.HasCheckConstraint("CK_RestaurantStaff_StaffRole", "[StaffRole] BETWEEN 0 AND 1"));
    }
}
