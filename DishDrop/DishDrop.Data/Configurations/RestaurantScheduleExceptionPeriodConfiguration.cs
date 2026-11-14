using DishDrop.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DishDrop.Data.Configurations;

public sealed class RestaurantScheduleExceptionPeriodConfiguration : IEntityTypeConfiguration<RestaurantScheduleExceptionPeriod>
{
    public void Configure(EntityTypeBuilder<RestaurantScheduleExceptionPeriod> b)
    {
        b.ToTable("RestaurantScheduleExceptionPeriod");
        b.HasKey(x => x.Id);
        b.HasOne(x => x.RestaurantScheduleException).WithMany().HasForeignKey(x => x.RestaurantScheduleExceptionId).OnDelete(DeleteBehavior.NoAction);
        b.ToTable(t => t.HasCheckConstraint("CK_RestaurantScheduleExceptionPeriod_Period", "([ClosesNextDay] = 0 AND [ClosesAt] > [OpensAt]) OR ([ClosesNextDay] = 1 AND [ClosesAt] < [OpensAt])"));
    }
}
