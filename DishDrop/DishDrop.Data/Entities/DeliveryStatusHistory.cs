using DishDrop.Data.Enums;

namespace DishDrop.Data.Entities;

public class DeliveryStatusHistory
{
    public int Id { get; set; }
    public int DeliveryId { get; set; }
    public DeliveryStatus? FromStatus { get; set; }
    public DeliveryStatus ToStatus { get; set; }
    public string? ChangedByUserId { get; set; }
    public DateTime ChangedAtUtc { get; set; }
    public string Reason { get; set; } = string.Empty;
    public Delivery Delivery { get; set; } = null!;
    public ApplicationUser? ChangedByUser { get; set; }
}
