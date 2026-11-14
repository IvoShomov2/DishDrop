using DishDrop.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DishDrop.Data.Configurations;

public sealed class OrderItemOptionConfiguration : IEntityTypeConfiguration<OrderItemOption>
{
    public void Configure(EntityTypeBuilder<OrderItemOption> b)
    {
        b.ToTable("OrderItemOption");
        b.HasKey(x => x.Id);
        b.Property(x => x.NameSnapshot).HasMaxLength(150).IsRequired();
        b.Property(x => x.AdditionalPrice).HasPrecision(18, 2);
        b.HasOne(x => x.OrderItem).WithMany().HasForeignKey(x => x.OrderItemId).OnDelete(DeleteBehavior.NoAction);
        b.HasOne(x => x.DishOption).WithMany().HasForeignKey(x => x.DishOptionId).OnDelete(DeleteBehavior.NoAction);
        b.HasIndex(x => new { x.OrderItemId, x.DishOptionId }).IsUnique();
        b.ToTable(t => t.HasCheckConstraint("CK_OrderItemOption_Price", "[AdditionalPrice] >= 0"));
    }
}
