using DishDrop.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DishDrop.Data.Configurations;

public sealed class MenuCategoryConfiguration : IEntityTypeConfiguration<MenuCategory>
{
    public void Configure(EntityTypeBuilder<MenuCategory> b)
    {
        b.ToTable("MenuCategory");
        b.HasKey(x => x.Id);
        b.Property(x => x.Name).HasMaxLength(150).IsRequired();
        b.Property(x => x.NormalizedName).HasMaxLength(150).IsRequired();
        b.HasOne(x => x.Menu).WithMany().HasForeignKey(x => x.MenuId).OnDelete(DeleteBehavior.NoAction);
        b.HasIndex(x => new { x.MenuId, x.NormalizedName }).IsUnique();
    }
}
