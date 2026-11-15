using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DishDrop.Data.Enums;
using static DishDrop.Data.DataConstants.Identity;

namespace DishDrop.Data.Entities
{
    [Table("CourierProfile")]
    public class CourierProfile
    {
        [Key]
        [Required]
        [MaxLength(UserIdMaxLength)]
        public string UserId { get; set; } = null!;

        public ApprovalStatus ApprovalStatus { get; set; }

        public VehicleType VehicleType { get; set; }

        public bool IsAvailable { get; set; } = true;

        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

        public DateTime? ApprovedAtUtc { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; } = [];

        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; } = null!;

        public ICollection<CourierDeliveryArea> DeliveryAreas { get; set; } = new List<CourierDeliveryArea>();

        public ICollection<CourierApprovalHistory> ApprovalHistory { get; set; } = new List<CourierApprovalHistory>();

        public ICollection<Delivery> Deliveries { get; set; } = new List<Delivery>();

        public ICollection<DeliveryAssignmentHistory> AssignmentHistory { get; set; } = new List<DeliveryAssignmentHistory>();
    }
}
