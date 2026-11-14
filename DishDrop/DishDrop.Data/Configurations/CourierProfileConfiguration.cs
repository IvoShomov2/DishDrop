using DishDrop.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DishDrop.Data.Configurations;

public sealed class CourierProfileConfiguration : IEntityTypeConfiguration<CourierProfile>
{
    public void Configure(EntityTypeBuilder<CourierProfile> b)
    {
        b.ToTable("CourierProfile");
        b.HasKey(x => x.UserId);
        b.Property(x => x.UserId).HasMaxLength(450).IsRequired();
        b.Property(x => x.RowVersion).IsRowVersion();
        b.HasOne(x => x.User).WithOne().HasForeignKey<CourierProfile>(x => x.UserId).OnDelete(DeleteBehavior.NoAction);
        b.ToTable(t => t.HasCheckConstraint("CK_CourierProfile_ApprovalStatus", "[ApprovalStatus] BETWEEN 0 AND 2"));
        b.ToTable(t => t.HasCheckConstraint("CK_CourierProfile_VehicleType", "[VehicleType] BETWEEN 0 AND 2"));
    }
}
