using DishDrop.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DishDrop.Data.Configurations;

public sealed class SupportTicketConfiguration : IEntityTypeConfiguration<SupportTicket>
{
    public void Configure(EntityTypeBuilder<SupportTicket> b)
    {
        b.ToTable("SupportTicket");
        b.HasKey(x => x.Id);
        b.Property(x => x.CreatedByUserId).HasMaxLength(450).IsRequired();
        b.Property(x => x.Subject).HasMaxLength(200).IsRequired();
        b.Property(x => x.AssignedAdminId).HasMaxLength(450);
        b.Property(x => x.RowVersion).IsRowVersion();
        b.HasOne(x => x.CreatedByUser).WithMany().HasForeignKey(x => x.CreatedByUserId).OnDelete(DeleteBehavior.NoAction);
        b.HasOne(x => x.Order).WithMany().HasForeignKey(x => x.OrderId).OnDelete(DeleteBehavior.NoAction);
        b.HasOne(x => x.AssignedAdmin).WithMany().HasForeignKey(x => x.AssignedAdminId).OnDelete(DeleteBehavior.NoAction);
        b.HasIndex(x => new { x.CreatedByUserId, x.CreatedAtUtc });
        b.HasIndex(x => new { x.Status, x.UpdatedAtUtc });
        b.ToTable(t => t.HasCheckConstraint("CK_SupportTicket_Category", "[Category] BETWEEN 0 AND 3"));
        b.ToTable(t => t.HasCheckConstraint("CK_SupportTicket_Status", "[Status] BETWEEN 0 AND 3"));
    }
}
