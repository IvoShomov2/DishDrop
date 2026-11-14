using DishDrop.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DishDrop.Data.Configurations;

public sealed class CuisineTypeConfiguration : IEntityTypeConfiguration<CuisineType>
{
    public void Configure(EntityTypeBuilder<CuisineType> b)
    {
        b.ToTable("CuisineType");
        b.HasKey(x => x.Id);
        b.Property(x => x.Name).HasMaxLength(100).IsRequired();
        b.Property(x => x.NormalizedName).HasMaxLength(100).IsRequired();
        b.HasIndex(x => x.NormalizedName).IsUnique();
    }
}
