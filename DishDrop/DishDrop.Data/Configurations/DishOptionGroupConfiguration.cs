using DishDrop.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DishDrop.Data.Configurations;

public sealed class DishOptionGroupConfiguration : IEntityTypeConfiguration<DishOptionGroup>
{
    public void Configure(EntityTypeBuilder<DishOptionGroup> b)
    {
        b.ToTable("DishOptionGroup");
        b.HasKey(x => x.Id);
        b.Property(x => x.Name).HasMaxLength(150).IsRequired();
        b.HasOne(x => x.Dish).WithMany().HasForeignKey(x => x.DishId).OnDelete(DeleteBehavior.NoAction);
        b.ToTable(t => t.HasCheckConstraint("CK_DishOptionGroup_Selections", "[MinSelections] >= 0 AND [MinSelections] <= [MaxSelections] AND [MaxSelections] > 0"));
    }
}
