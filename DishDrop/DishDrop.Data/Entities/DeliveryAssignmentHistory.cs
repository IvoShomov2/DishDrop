using DishDrop.Data.Enums;

namespace DishDrop.Data.Entities;

public class DeliveryAssignmentHistory
{
    public int Id { get; set; }
    public int DeliveryId { get; set; }
    public string CourierId { get; set; } = string.Empty;
    public DateTime AssignedAtUtc { get; set; }
    public DateTime? ReleasedAtUtc { get; set; }
    public string? ReleaseReason { get; set; }
    public Delivery Delivery { get; set; } = null!;
    public CourierProfile Courier { get; set; } = null!;
}
