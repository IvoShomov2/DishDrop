using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DishDrop.Data.Configurations;

// Preserve the original schema independently of Identity UI/CLI service registration.
public sealed class IdentityLoginKeyConfiguration : IEntityTypeConfiguration<IdentityUserLogin<string>>
{
    public void Configure(EntityTypeBuilder<IdentityUserLogin<string>> b)
    {
        b.Property(x => x.LoginProvider).HasMaxLength(128);
        b.Property(x => x.ProviderKey).HasMaxLength(128);
    }
}

public sealed class IdentityTokenKeyConfiguration : IEntityTypeConfiguration<IdentityUserToken<string>>
{
    public void Configure(EntityTypeBuilder<IdentityUserToken<string>> b)
    {
        b.Property(x => x.LoginProvider).HasMaxLength(128);
        b.Property(x => x.Name).HasMaxLength(128);
    }
}
