using DishDrop.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DishDrop.Data.Configurations;

public sealed class CartItemOptionConfiguration : IEntityTypeConfiguration<CartItemOption>
{
    public void Configure(EntityTypeBuilder<CartItemOption> b)
    {
        b.ToTable("CartItemOption");
        b.HasKey(x => new { x.CartItemId, x.DishOptionId });
        b.HasOne(x => x.CartItem).WithMany().HasForeignKey(x => x.CartItemId).OnDelete(DeleteBehavior.NoAction);
        b.HasOne(x => x.DishOption).WithMany().HasForeignKey(x => x.DishOptionId).OnDelete(DeleteBehavior.NoAction);
    }
}
