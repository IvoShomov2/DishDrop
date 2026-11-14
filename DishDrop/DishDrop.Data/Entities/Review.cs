using DishDrop.Data.Enums;

namespace DishDrop.Data.Entities;

public class Review
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public int RestaurantRating { get; set; }
    public int? DeliveryRating { get; set; }
    public string? Comment { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAtUtc { get; set; } = DateTime.UtcNow;
    public ModerationStatus ModerationStatus { get; set; }
    public byte[] RowVersion { get; set; } = [];
    public Order Order { get; set; } = null!;
}
