using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DishDrop.Data.Enums;

namespace DishDrop.Data.Entities;

[Table("CourierApprovalHistory")]
public class CourierApprovalHistory
{
    [Key]
    public int Id { get; set; }
    [Required]
    [MaxLength(DataConstants.Identity.UserIdMaxLength)]
    public string CourierId { get; set; } = string.Empty;
    public ApprovalStatus? FromStatus { get; set; }
    public ApprovalStatus ToStatus { get; set; }
    [Required]
    [MaxLength(DataConstants.Identity.UserIdMaxLength)]
    public string ChangedByUserId { get; set; } = string.Empty;
    public DateTime ChangedAtUtc { get; set; }
    [Required]
    [MaxLength(DataConstants.CourierApprovalHistory.ReasonMaxLength)]
    public string Reason { get; set; } = string.Empty;
    [ForeignKey(nameof(CourierId))]
    public CourierProfile Courier { get; set; } = null!;
    [ForeignKey(nameof(ChangedByUserId))]
    public ApplicationUser ChangedByUser { get; set; } = null!;
}
