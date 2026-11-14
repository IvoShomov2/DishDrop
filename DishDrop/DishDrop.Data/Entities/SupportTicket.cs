using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DishDrop.Data.Enums;

namespace DishDrop.Data.Entities;

[Table("SupportTicket")]
public class SupportTicket
{
    [Key]
    public int Id { get; set; }
    [Required]
    [MaxLength(DataConstants.Identity.UserIdMaxLength)]
    public string CreatedByUserId { get; set; } = string.Empty;
    public int? OrderId { get; set; }
    public SupportCategory Category { get; set; }
    [Required]
    [MaxLength(DataConstants.SupportTicket.SubjectMaxLength)]
    public string Subject { get; set; } = string.Empty;
    public SupportStatus Status { get; set; }
    [MaxLength(DataConstants.Identity.UserIdMaxLength)]
    public string? AssignedAdminId { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAtUtc { get; set; } = DateTime.UtcNow;
    [Timestamp]
    public byte[] RowVersion { get; set; } = [];
    [ForeignKey(nameof(CreatedByUserId))]
    public ApplicationUser CreatedByUser { get; set; } = null!;
    [ForeignKey(nameof(OrderId))]
    public Order? Order { get; set; }
    [ForeignKey(nameof(AssignedAdminId))]
    public ApplicationUser? AssignedAdmin { get; set; }
}
