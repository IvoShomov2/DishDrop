using DishDrop.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DishDrop.Data.Configurations;

public sealed class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> b)
    {
        b.ToTable("Notification");
        b.HasKey(x => x.Id);
        b.Property(x => x.RecipientUserId).HasMaxLength(450).IsRequired();
        b.Property(x => x.Title).HasMaxLength(200).IsRequired();
        b.Property(x => x.Message).HasMaxLength(2000).IsRequired();
        b.HasOne(x => x.RecipientUser).WithMany().HasForeignKey(x => x.RecipientUserId).OnDelete(DeleteBehavior.NoAction);
        b.HasOne(x => x.Order).WithMany().HasForeignKey(x => x.OrderId).OnDelete(DeleteBehavior.NoAction);
        b.HasOne(x => x.SupportTicket).WithMany().HasForeignKey(x => x.SupportTicketId).OnDelete(DeleteBehavior.NoAction);
        b.HasIndex(x => new { x.RecipientUserId, x.CreatedAtUtc });
        b.HasIndex(x => new { x.RecipientUserId, x.CreatedAtUtc }, "UnreadNotifications").HasDatabaseName("IX_Notification_Unread").HasFilter("[ReadAtUtc] IS NULL");
        b.ToTable(t => t.HasCheckConstraint("CK_Notification_Type", "[Type] BETWEEN 0 AND 4"));
    }
}
