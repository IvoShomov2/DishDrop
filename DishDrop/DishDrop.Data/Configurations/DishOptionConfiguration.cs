using DishDrop.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DishDrop.Data.Configurations;

public sealed class DishOptionConfiguration : IEntityTypeConfiguration<DishOption>
{
    public void Configure(EntityTypeBuilder<DishOption> b)
    {
        b.ToTable("DishOption");
        b.HasKey(x => x.Id);
        b.Property(x => x.Name).HasMaxLength(150).IsRequired();
        b.Property(x => x.AdditionalPrice).HasPrecision(18, 2);
        b.HasOne(x => x.DishOptionGroup).WithMany().HasForeignKey(x => x.DishOptionGroupId).OnDelete(DeleteBehavior.NoAction);
        b.ToTable(t => t.HasCheckConstraint("CK_DishOption_Price", "[AdditionalPrice] >= 0"));
    }
}
