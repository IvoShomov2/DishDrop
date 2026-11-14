using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DishDrop.Data.Enums;

namespace DishDrop.Data.Entities;

[Table("AuditLog")]
public class AuditLog
{
    [Key]
    public int Id { get; set; }
    [MaxLength(DataConstants.Identity.UserIdMaxLength)]
    public string? ActorUserId { get; set; }
    [Required]
    [MaxLength(DataConstants.AuditLog.ActionMaxLength)]
    public string Action { get; set; } = string.Empty;
    [Required]
    [MaxLength(DataConstants.AuditLog.EntityTypeMaxLength)]
    public string EntityType { get; set; } = string.Empty;
    [Required]
    [MaxLength(DataConstants.AuditLog.EntityIdMaxLength)]
    public string EntityId { get; set; } = string.Empty;
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    [MaxLength(DataConstants.AuditLog.ReasonMaxLength)]
    public string? Reason { get; set; }
    [MaxLength(DataConstants.AuditLog.CorrelationIdMaxLength)]
    public string? CorrelationId { get; set; }
    [ForeignKey(nameof(ActorUserId))]
    public ApplicationUser? ActorUser { get; set; }
}
