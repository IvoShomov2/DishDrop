using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DishDrop.Data.Enums;

namespace DishDrop.Data.Entities;

[Table("Notification")]
public class Notification
{
    [Key]
    public int Id { get; set; }
    [Required]
    [MaxLength(DataConstants.Identity.UserIdMaxLength)]
    public string RecipientUserId { get; set; } = string.Empty;
    public NotificationType Type { get; set; }
    [Required]
    [MaxLength(DataConstants.Notification.TitleMaxLength)]
    public string Title { get; set; } = string.Empty;
    [Required]
    [MaxLength(DataConstants.Notification.MessageMaxLength)]
    public string Message { get; set; } = string.Empty;
    public int? OrderId { get; set; }
    public int? SupportTicketId { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime? ReadAtUtc { get; set; }
    [ForeignKey(nameof(RecipientUserId))]
    public ApplicationUser RecipientUser { get; set; } = null!;
    [ForeignKey(nameof(OrderId))]
    public Order? Order { get; set; }
    [ForeignKey(nameof(SupportTicketId))]
    public SupportTicket? SupportTicket { get; set; }
}
