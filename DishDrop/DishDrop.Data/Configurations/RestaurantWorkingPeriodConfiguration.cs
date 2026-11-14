using DishDrop.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DishDrop.Data.Configurations;

public sealed class RestaurantWorkingPeriodConfiguration : IEntityTypeConfiguration<RestaurantWorkingPeriod>
{
    public void Configure(EntityTypeBuilder<RestaurantWorkingPeriod> b)
    {
        b.ToTable("RestaurantWorkingPeriod");
        b.HasKey(x => x.Id);
        b.HasOne(x => x.Restaurant).WithMany().HasForeignKey(x => x.RestaurantId).OnDelete(DeleteBehavior.NoAction);
        b.HasIndex(x => new { x.RestaurantId, x.DayOfWeek });
        b.ToTable(t => t.HasCheckConstraint("CK_RestaurantWorkingPeriod_Period", "([ClosesNextDay] = 0 AND [ClosesAt] > [OpensAt]) OR ([ClosesNextDay] = 1 AND [ClosesAt] < [OpensAt])"));
        b.ToTable(t => t.HasCheckConstraint("CK_RestaurantWorkingPeriod_DayOfWeek", "[DayOfWeek] BETWEEN 0 AND 6"));
    }
}
