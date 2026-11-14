using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DishDrop.Data.Enums;

namespace DishDrop.Data.Entities;

[Table("Cart")]
public class Cart
{
    [Key]
    public int Id { get; set; }
    [Required]
    [MaxLength(DataConstants.Identity.UserIdMaxLength)]
    public string UserId { get; set; } = string.Empty;
    public int? RestaurantId { get; set; }
    public DateTime UpdatedAtUtc { get; set; } = DateTime.UtcNow;
    [Timestamp]
    public byte[] RowVersion { get; set; } = [];
    [ForeignKey(nameof(UserId))]
    public ApplicationUser User { get; set; } = null!;
    [ForeignKey(nameof(RestaurantId))]
    public Restaurant? Restaurant { get; set; }
}
