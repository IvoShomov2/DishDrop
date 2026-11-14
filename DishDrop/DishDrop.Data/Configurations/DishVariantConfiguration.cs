using DishDrop.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DishDrop.Data.Configurations;

public sealed class DishVariantConfiguration : IEntityTypeConfiguration<DishVariant>
{
    public void Configure(EntityTypeBuilder<DishVariant> b)
    {
        b.ToTable("DishVariant");
        b.HasKey(x => x.Id);
        b.Property(x => x.Name).HasMaxLength(100).IsRequired();
        b.Property(x => x.NormalizedName).HasMaxLength(100).IsRequired();
        b.Property(x => x.Price).HasPrecision(18, 2);
        b.Property(x => x.PortionQuantity).HasPrecision(18, 3);
        b.HasOne(x => x.Dish).WithMany().HasForeignKey(x => x.DishId).OnDelete(DeleteBehavior.NoAction);
        b.HasIndex(x => new { x.DishId, x.NormalizedName }).IsUnique();
        b.ToTable(t => t.HasCheckConstraint("CK_DishVariant_Price", "[Price] > 0"));
        b.ToTable(t => t.HasCheckConstraint("CK_DishVariant_Portion", "([PortionQuantity] IS NULL AND [PortionUnit] IS NULL) OR ([PortionQuantity] IS NOT NULL AND [PortionUnit] IS NOT NULL AND [PortionQuantity] > 0)"));
        b.ToTable(t => t.HasCheckConstraint("CK_DishVariant_PortionUnit", "[PortionUnit] IS NULL OR [PortionUnit] BETWEEN 0 AND 2"));
    }
}
