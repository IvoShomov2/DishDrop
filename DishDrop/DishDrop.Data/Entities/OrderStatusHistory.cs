using DishDrop.Data.Enums;

namespace DishDrop.Data.Entities;

public class OrderStatusHistory
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public OrderStatus? FromStatus { get; set; }
    public OrderStatus ToStatus { get; set; }
    public string? ChangedByUserId { get; set; }
    public DateTime ChangedAtUtc { get; set; }
    public string Reason { get; set; } = string.Empty;
    public Order Order { get; set; } = null!;
    public ApplicationUser? ChangedByUser { get; set; }
}
