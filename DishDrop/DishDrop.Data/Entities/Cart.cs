using DishDrop.Data.Enums;

namespace DishDrop.Data.Entities;

public class Cart
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public int? RestaurantId { get; set; }
    public DateTime UpdatedAtUtc { get; set; } = DateTime.UtcNow;
    public byte[] RowVersion { get; set; } = [];
    public ApplicationUser User { get; set; } = null!;
    public Restaurant? Restaurant { get; set; }
}
