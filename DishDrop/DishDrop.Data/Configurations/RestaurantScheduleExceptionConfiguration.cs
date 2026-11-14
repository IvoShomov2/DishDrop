using DishDrop.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DishDrop.Data.Configurations;

public sealed class RestaurantScheduleExceptionConfiguration : IEntityTypeConfiguration<RestaurantScheduleException>
{
    public void Configure(EntityTypeBuilder<RestaurantScheduleException> b)
    {
        b.ToTable("RestaurantScheduleException");
        b.HasKey(x => x.Id);
        b.HasOne(x => x.Restaurant).WithMany().HasForeignKey(x => x.RestaurantId).OnDelete(DeleteBehavior.NoAction);
        b.HasIndex(x => new { x.RestaurantId, x.Date }).IsUnique();
    }
}
