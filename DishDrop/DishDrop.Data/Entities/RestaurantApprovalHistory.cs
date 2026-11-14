using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DishDrop.Data.Enums;
using static DishDrop.Data.DataConstants.RestaurantApprovalHistory;
using static DishDrop.Data.DataConstants.Identity;

namespace DishDrop.Data.Entities
{
    [Table("RestaurantApprovalHistory")]
    public class RestaurantApprovalHistory
    {
        [Key]
        public int Id { get; set; }

        public int RestaurantId { get; set; }

        public ApprovalStatus? FromStatus { get; set; }

        public ApprovalStatus ToStatus { get; set; }

        [Required]
        [MaxLength(UserIdMaxLength)]
        public string ChangedByUserId { get; set; } = null!;

        public DateTime ChangedAtUtc { get; set; }

        [Required]
        [MaxLength(ReasonMaxLength)]
        public string Reason { get; set; } = null!;

        [ForeignKey(nameof(RestaurantId))]
        public Restaurant Restaurant { get; set; } = null!;

        [ForeignKey(nameof(ChangedByUserId))]
        public ApplicationUser ChangedByUser { get; set; } = null!;
    }
}
