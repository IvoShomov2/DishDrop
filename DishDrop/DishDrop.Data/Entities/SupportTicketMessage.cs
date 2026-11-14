using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DishDrop.Data.Enums;

namespace DishDrop.Data.Entities;

[Table("SupportTicketMessage")]
public class SupportTicketMessage
{
    [Key]
    public int Id { get; set; }
    public int SupportTicketId { get; set; }
    [Required]
    [MaxLength(DataConstants.Identity.UserIdMaxLength)]
    public string AuthorUserId { get; set; } = string.Empty;
    [Required]
    [MaxLength(DataConstants.SupportTicketMessage.TextMaxLength)]
    public string Text { get; set; } = string.Empty;
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    [ForeignKey(nameof(SupportTicketId))]
    public SupportTicket SupportTicket { get; set; } = null!;
    [ForeignKey(nameof(AuthorUserId))]
    public ApplicationUser AuthorUser { get; set; } = null!;
}
