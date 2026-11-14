using DishDrop.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DishDrop.Data.Configurations;

public sealed class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> b)
    {
        b.ToTable("OrderItem");
        b.HasKey(x => x.Id);
        b.Property(x => x.DishName).HasMaxLength(150).IsRequired();
        b.Property(x => x.VariantName).HasMaxLength(100).IsRequired();
        b.Property(x => x.BaseUnitPrice).HasPrecision(18, 2);
        b.Property(x => x.OptionsUnitPrice).HasPrecision(18, 2);
        b.Property(x => x.LineTotal).HasPrecision(18, 2);
        b.Property(x => x.SpecialInstructions).HasMaxLength(1000);
        b.HasOne(x => x.Order).WithMany().HasForeignKey(x => x.OrderId).OnDelete(DeleteBehavior.NoAction);
        b.HasOne(x => x.DishVariant).WithMany().HasForeignKey(x => x.DishVariantId).OnDelete(DeleteBehavior.NoAction);
        b.ToTable(t => t.HasCheckConstraint("CK_OrderItem_Amounts", "[BaseUnitPrice] > 0 AND [OptionsUnitPrice] >= 0 AND [LineTotal] = ([BaseUnitPrice] + [OptionsUnitPrice]) * [Quantity]"));
        b.ToTable(t => t.HasCheckConstraint("CK_OrderItem_Quantity", "[Quantity] BETWEEN 1 AND 99"));
    }
}
