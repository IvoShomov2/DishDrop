using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DishDrop.Data.Enums;

namespace DishDrop.Data.Entities;

[Table("RestaurantApprovalHistory")]
public class RestaurantApprovalHistory
{
    [Key]
    public int Id { get; set; }
    public int RestaurantId { get; set; }
    public ApprovalStatus? FromStatus { get; set; }
    public ApprovalStatus ToStatus { get; set; }
    [Required]
    [MaxLength(DataConstants.Identity.UserIdMaxLength)]
    public string ChangedByUserId { get; set; } = string.Empty;
    public DateTime ChangedAtUtc { get; set; }
    [Required]
    [MaxLength(DataConstants.RestaurantApprovalHistory.ReasonMaxLength)]
    public string Reason { get; set; } = string.Empty;
    [ForeignKey(nameof(RestaurantId))]
    public Restaurant Restaurant { get; set; } = null!;
    [ForeignKey(nameof(ChangedByUserId))]
    public ApplicationUser ChangedByUser { get; set; } = null!;
}
