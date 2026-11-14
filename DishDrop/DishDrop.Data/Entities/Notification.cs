using DishDrop.Data.Enums;

namespace DishDrop.Data.Entities;

public class Notification
{
    public int Id { get; set; }
    public string RecipientUserId { get; set; } = string.Empty;
    public NotificationType Type { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public int? OrderId { get; set; }
    public int? SupportTicketId { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime? ReadAtUtc { get; set; }
    public ApplicationUser RecipientUser { get; set; } = null!;
    public Order? Order { get; set; }
    public SupportTicket? SupportTicket { get; set; }
}
