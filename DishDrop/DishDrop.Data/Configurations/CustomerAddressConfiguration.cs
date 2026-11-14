using DishDrop.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DishDrop.Data.Configurations;

public sealed class CustomerAddressConfiguration : IEntityTypeConfiguration<CustomerAddress>
{
    public void Configure(EntityTypeBuilder<CustomerAddress> b)
    {
        b.ToTable("CustomerAddress");
        b.HasKey(x => x.Id);
        b.Property(x => x.UserId).HasMaxLength(450).IsRequired();
        b.Property(x => x.Label).HasMaxLength(100).IsRequired();
        b.Property(x => x.StreetAddress).HasMaxLength(300).IsRequired();
        b.Property(x => x.Building).HasMaxLength(30);
        b.Property(x => x.Entrance).HasMaxLength(30);
        b.Property(x => x.Floor).HasMaxLength(30);
        b.Property(x => x.Apartment).HasMaxLength(30);
        b.Property(x => x.DeliveryInstructions).HasMaxLength(1000);
        b.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.NoAction);
        b.HasOne(x => x.DeliveryArea).WithMany().HasForeignKey(x => x.DeliveryAreaId).OnDelete(DeleteBehavior.NoAction);
        b.HasIndex(x => x.UserId).IsUnique().HasFilter("[IsDefault] = 1");
    }
}
