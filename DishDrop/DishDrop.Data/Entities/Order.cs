using DishDrop.Data.Enums;

namespace DishDrop.Data.Entities;

public class Order
{
    public int Id { get; set; }
    public string OrderNumber { get; set; } = string.Empty;
    public string CustomerId { get; set; } = string.Empty;
    public int RestaurantId { get; set; }
    public OrderStatus Status { get; set; }
    public decimal Subtotal { get; set; }
    public decimal DeliveryFee { get; set; }
    public decimal TotalAmount { get; set; }
    public string CurrencyCode { get; set; } = "EUR";
    public string? CustomerNote { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime? EstimatedReadyAtUtc { get; set; }
    public Guid CheckoutToken { get; set; }
    public string CheckoutRequestHash { get; set; } = string.Empty;
    public string RestaurantNameSnapshot { get; set; } = string.Empty;
    public string PickupCitySnapshot { get; set; } = string.Empty;
    public string PickupAddressSnapshot { get; set; } = string.Empty;
    public string PickupPhoneSnapshot { get; set; } = string.Empty;
    public byte[] RowVersion { get; set; } = [];
    public ApplicationUser Customer { get; set; } = null!;
    public Restaurant Restaurant { get; set; } = null!;
}
