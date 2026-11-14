using DishDrop.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DishDrop.Data.Configurations;

public sealed class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
{
    public void Configure(EntityTypeBuilder<AuditLog> b)
    {
        b.ToTable("AuditLog");
        b.HasKey(x => x.Id);
        b.Property(x => x.ActorUserId).HasMaxLength(450);
        b.Property(x => x.Action).HasMaxLength(100).IsRequired();
        b.Property(x => x.EntityType).HasMaxLength(100).IsRequired();
        b.Property(x => x.EntityId).HasMaxLength(450).IsRequired();
        b.Property(x => x.Reason).HasMaxLength(1000);
        b.Property(x => x.CorrelationId).HasMaxLength(100);
        b.HasOne(x => x.ActorUser).WithMany().HasForeignKey(x => x.ActorUserId).OnDelete(DeleteBehavior.NoAction);
        b.HasIndex(x => new { x.EntityType, x.EntityId, x.CreatedAtUtc });
        b.HasIndex(x => new { x.ActorUserId, x.CreatedAtUtc });
    }
}
