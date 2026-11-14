using DishDrop.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DishDrop.Data.Configurations;

public sealed class RestaurantCuisineTypeConfiguration : IEntityTypeConfiguration<RestaurantCuisineType>
{
    public void Configure(EntityTypeBuilder<RestaurantCuisineType> b)
    {
        b.ToTable("RestaurantCuisineType");
        b.HasKey(x => new { x.RestaurantId, x.CuisineTypeId });
        b.HasOne(x => x.Restaurant).WithMany().HasForeignKey(x => x.RestaurantId).OnDelete(DeleteBehavior.NoAction);
        b.HasOne(x => x.CuisineType).WithMany().HasForeignKey(x => x.CuisineTypeId).OnDelete(DeleteBehavior.NoAction);
    }
}
