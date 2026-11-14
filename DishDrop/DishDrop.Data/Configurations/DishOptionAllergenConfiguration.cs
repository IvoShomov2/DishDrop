using DishDrop.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DishDrop.Data.Configurations;

public sealed class DishOptionAllergenConfiguration : IEntityTypeConfiguration<DishOptionAllergen>
{
    public void Configure(EntityTypeBuilder<DishOptionAllergen> b)
    {
        b.ToTable("DishOptionAllergen");
        b.HasKey(x => new { x.DishOptionId, x.AllergenId });
        b.HasOne(x => x.DishOption).WithMany().HasForeignKey(x => x.DishOptionId).OnDelete(DeleteBehavior.NoAction);
        b.HasOne(x => x.Allergen).WithMany().HasForeignKey(x => x.AllergenId).OnDelete(DeleteBehavior.NoAction);
    }
}
