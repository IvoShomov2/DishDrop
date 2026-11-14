using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static DishDrop.Data.DataConstants.AuditLog;
using static DishDrop.Data.DataConstants.Identity;

namespace DishDrop.Data.Entities
{
    [Table("AuditLog")]
    public class AuditLog
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(UserIdMaxLength)]
        public string? ActorUserId { get; set; }

        [Required]
        [MaxLength(ActionMaxLength)]
        public string Action { get; set; } = null!;

        [Required]
        [MaxLength(EntityTypeMaxLength)]
        public string EntityType { get; set; } = null!;

        [Required]
        [MaxLength(EntityIdMaxLength)]
        public string EntityId { get; set; } = null!;

        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

        [MaxLength(ReasonMaxLength)]
        public string? Reason { get; set; }

        [MaxLength(CorrelationIdMaxLength)]
        public string? CorrelationId { get; set; }

        [ForeignKey(nameof(ActorUserId))]
        public ApplicationUser? ActorUser { get; set; }
    }
}
