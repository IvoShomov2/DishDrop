using DishDrop.Data.Enums;

namespace DishDrop.Data.Entities;

public class RestaurantStaff
{
    public int RestaurantId { get; set; }
    public string UserId { get; set; } = string.Empty;
    public StaffRole StaffRole { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public Restaurant Restaurant { get; set; } = null!;
    public ApplicationUser User { get; set; } = null!;
}
