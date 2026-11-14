using DishDrop.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DishDrop.Data.Configurations;

public sealed class SupportTicketMessageConfiguration : IEntityTypeConfiguration<SupportTicketMessage>
{
    public void Configure(EntityTypeBuilder<SupportTicketMessage> b)
    {
        b.ToTable("SupportTicketMessage");
        b.HasKey(x => x.Id);
        b.Property(x => x.AuthorUserId).HasMaxLength(450).IsRequired();
        b.Property(x => x.Text).HasMaxLength(4000).IsRequired();
        b.HasOne(x => x.SupportTicket).WithMany().HasForeignKey(x => x.SupportTicketId).OnDelete(DeleteBehavior.NoAction);
        b.HasOne(x => x.AuthorUser).WithMany().HasForeignKey(x => x.AuthorUserId).OnDelete(DeleteBehavior.NoAction);
        b.HasIndex(x => new { x.SupportTicketId, x.CreatedAtUtc });
    }
}
