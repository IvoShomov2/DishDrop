using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static DishDrop.Data.DataConstants.DeliveryAssignmentHistory;
using static DishDrop.Data.DataConstants.Identity;

namespace DishDrop.Data.Entities
{
    [Table("DeliveryAssignmentHistory")]
    public class DeliveryAssignmentHistory
    {
        [Key]
        public int Id { get; set; }

        public int DeliveryId { get; set; }

        [Required]
        [MaxLength(UserIdMaxLength)]
        public string CourierId { get; set; } = null!;

        public DateTime AssignedAtUtc { get; set; }

        public DateTime? ReleasedAtUtc { get; set; }

        [MaxLength(ReleaseReasonMaxLength)]
        public string? ReleaseReason { get; set; }

        [ForeignKey(nameof(DeliveryId))]
        public Delivery Delivery { get; set; } = null!;

        [ForeignKey(nameof(CourierId))]
        public CourierProfile Courier { get; set; } = null!;
    }
}
