using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DishDrop.Data.Enums;

namespace DishDrop.Data.Entities;

[Table("RestaurantStaff")]
public class RestaurantStaff
{
    public int RestaurantId { get; set; }
    [Required]
    [MaxLength(DataConstants.Identity.UserIdMaxLength)]
    public string UserId { get; set; } = string.Empty;
    public StaffRole StaffRole { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    [ForeignKey(nameof(RestaurantId))]
    public Restaurant Restaurant { get; set; } = null!;
    [ForeignKey(nameof(UserId))]
    public ApplicationUser User { get; set; } = null!;
}
