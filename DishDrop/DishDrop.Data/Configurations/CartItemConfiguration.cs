using DishDrop.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DishDrop.Data.Configurations;

public sealed class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
{
    public void Configure(EntityTypeBuilder<CartItem> b)
    {
        b.ToTable("CartItem");
        b.HasKey(x => x.Id);
        b.Property(x => x.SpecialInstructions).HasMaxLength(1000);
        b.HasOne(x => x.Cart).WithMany().HasForeignKey(x => x.CartId).OnDelete(DeleteBehavior.NoAction);
        b.HasOne(x => x.DishVariant).WithMany().HasForeignKey(x => x.DishVariantId).OnDelete(DeleteBehavior.NoAction);
        b.ToTable(t => t.HasCheckConstraint("CK_CartItem_Quantity", "[Quantity] BETWEEN 1 AND 99"));
    }
}
