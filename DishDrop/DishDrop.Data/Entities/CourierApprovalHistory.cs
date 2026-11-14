using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DishDrop.Data.Enums;
using static DishDrop.Data.DataConstants.CourierApprovalHistory;
using static DishDrop.Data.DataConstants.Identity;

namespace DishDrop.Data.Entities
{
    [Table("CourierApprovalHistory")]
    public class CourierApprovalHistory
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(UserIdMaxLength)]
        public string CourierId { get; set; } = null!;

        public ApprovalStatus? FromStatus { get; set; }

        public ApprovalStatus ToStatus { get; set; }

        [Required]
        [MaxLength(UserIdMaxLength)]
        public string ChangedByUserId { get; set; } = null!;

        public DateTime ChangedAtUtc { get; set; }

        [Required]
        [MaxLength(ReasonMaxLength)]
        public string Reason { get; set; } = null!;

        [ForeignKey(nameof(CourierId))]
        public CourierProfile Courier { get; set; } = null!;

        [ForeignKey(nameof(ChangedByUserId))]
        public ApplicationUser ChangedByUser { get; set; } = null!;
    }
}
