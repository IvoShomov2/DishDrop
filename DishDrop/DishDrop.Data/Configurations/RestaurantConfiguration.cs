using DishDrop.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DishDrop.Data.Configurations;

public sealed class RestaurantConfiguration : IEntityTypeConfiguration<Restaurant>
{
    public void Configure(EntityTypeBuilder<Restaurant> b)
    {
        b.ToTable("Restaurant");
        b.HasKey(x => x.Id);
        b.Property(x => x.Name).HasMaxLength(150).IsRequired();
        b.Property(x => x.Slug).HasMaxLength(180).IsRequired();
        b.Property(x => x.Description).HasMaxLength(2000).IsRequired();
        b.Property(x => x.Address).HasMaxLength(300).IsRequired();
        b.Property(x => x.PhoneNumber).HasMaxLength(30).IsRequired();
        b.Property(x => x.ImagePath).HasMaxLength(500);
        b.Property(x => x.MinimumOrderAmount).HasPrecision(18, 2);
        b.Property(x => x.RowVersion).IsRowVersion();
        b.HasOne(x => x.City).WithMany().HasForeignKey(x => x.CityId).OnDelete(DeleteBehavior.NoAction);
        b.HasIndex(x => x.Slug).IsUnique();
        b.ToTable(t => t.HasCheckConstraint("CK_Restaurant_MinimumOrder", "[MinimumOrderAmount] >= 0"));
        b.ToTable(t => t.HasCheckConstraint("CK_Restaurant_Preparation", "[EstimatedPreparationMinutes] > 0"));
        b.ToTable(t => t.HasCheckConstraint("CK_Restaurant_ApprovalStatus", "[ApprovalStatus] BETWEEN 0 AND 2"));
    }
}
