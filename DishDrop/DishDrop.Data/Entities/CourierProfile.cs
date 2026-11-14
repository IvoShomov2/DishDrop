using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DishDrop.Data.Enums;

namespace DishDrop.Data.Entities;

[Table("CourierProfile")]
public class CourierProfile
{
    [Key]
    [Required]
    [MaxLength(DataConstants.Identity.UserIdMaxLength)]
    public string UserId { get; set; } = string.Empty;
    public ApprovalStatus ApprovalStatus { get; set; }
    public VehicleType VehicleType { get; set; }
    public bool IsAvailable { get; set; } = true;
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime? ApprovedAtUtc { get; set; }
    [Timestamp]
    public byte[] RowVersion { get; set; } = [];
    [ForeignKey(nameof(UserId))]
    public ApplicationUser User { get; set; } = null!;
}
