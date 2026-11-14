using DishDrop.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DishDrop.Data.Configurations;

public sealed class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> b)
    {
        b.ToTable("Payment");
        b.HasKey(x => x.Id);
        b.Property(x => x.Amount).HasPrecision(18, 2);
        b.Property(x => x.CurrencyCode).HasMaxLength(3).IsRequired();
        b.Property(x => x.CollectedByUserId).HasMaxLength(450);
        b.Property(x => x.RowVersion).IsRowVersion();
        b.HasOne(x => x.Order).WithMany().HasForeignKey(x => x.OrderId).OnDelete(DeleteBehavior.NoAction);
        b.HasOne(x => x.CollectedByUser).WithMany().HasForeignKey(x => x.CollectedByUserId).OnDelete(DeleteBehavior.NoAction);
        b.HasIndex(x => x.OrderId).IsUnique();
        b.ToTable(t => t.HasCheckConstraint("CK_Payment_Amount", "[Amount] >= 0"));
        b.ToTable(t => t.HasCheckConstraint("CK_Payment_Currency", "LEN([CurrencyCode]) = 3 AND [CurrencyCode] COLLATE Latin1_General_100_BIN2 NOT LIKE '%[^A-Z]%'"));
        b.ToTable(t => t.HasCheckConstraint("CK_Payment_Method", "[Method] BETWEEN 0 AND 0"));
        b.ToTable(t => t.HasCheckConstraint("CK_Payment_Status", "[Status] BETWEEN 0 AND 3"));
    }
}
