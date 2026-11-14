using DishDrop.Data.Enums;

namespace DishDrop.Data.Entities;

public class SupportTicketMessage
{
    public int Id { get; set; }
    public int SupportTicketId { get; set; }
    public string AuthorUserId { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public SupportTicket SupportTicket { get; set; } = null!;
    public ApplicationUser AuthorUser { get; set; } = null!;
}
