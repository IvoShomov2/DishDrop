using DishDrop.Data.Enums;

namespace DishDrop.Data.Entities;

public class Delivery
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public string? CourierId { get; set; }
    public int DeliveryAreaId { get; set; }
    public DeliveryStatus Status { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime? AssignedAtUtc { get; set; }
    public DateTime? PickedUpAtUtc { get; set; }
    public DateTime? DeliveredAtUtc { get; set; }
    public byte[] RowVersion { get; set; } = [];
    public Order Order { get; set; } = null!;
    public CourierProfile? Courier { get; set; }
    public DeliveryArea DeliveryArea { get; set; } = null!;
}
