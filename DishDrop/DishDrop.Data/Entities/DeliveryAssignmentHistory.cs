using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DishDrop.Data.Enums;

namespace DishDrop.Data.Entities;

[Table("DeliveryAssignmentHistory")]
public class DeliveryAssignmentHistory
{
    [Key]
    public int Id { get; set; }
    public int DeliveryId { get; set; }
    [Required]
    [MaxLength(DataConstants.Identity.UserIdMaxLength)]
    public string CourierId { get; set; } = string.Empty;
    public DateTime AssignedAtUtc { get; set; }
    public DateTime? ReleasedAtUtc { get; set; }
    [MaxLength(DataConstants.DeliveryAssignmentHistory.ReleaseReasonMaxLength)]
    public string? ReleaseReason { get; set; }
    [ForeignKey(nameof(DeliveryId))]
    public Delivery Delivery { get; set; } = null!;
    [ForeignKey(nameof(CourierId))]
    public CourierProfile Courier { get; set; } = null!;
}
