using DishDrop.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DishDrop.Data.Configurations;

public sealed class DishAllergenConfiguration : IEntityTypeConfiguration<DishAllergen>
{
    public void Configure(EntityTypeBuilder<DishAllergen> b)
    {
        b.ToTable("DishAllergen");
        b.HasKey(x => new { x.DishId, x.AllergenId });
        b.HasOne(x => x.Dish).WithMany().HasForeignKey(x => x.DishId).OnDelete(DeleteBehavior.NoAction);
        b.HasOne(x => x.Allergen).WithMany().HasForeignKey(x => x.AllergenId).OnDelete(DeleteBehavior.NoAction);
    }
}
