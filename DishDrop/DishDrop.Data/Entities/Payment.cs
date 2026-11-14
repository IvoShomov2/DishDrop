using DishDrop.Data.Enums;

namespace DishDrop.Data.Entities;

public class Payment
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public PaymentMethod Method { get; set; }
    public PaymentStatus Status { get; set; }
    public decimal Amount { get; set; }
    public string CurrencyCode { get; set; } = "EUR";
    public DateTime? CollectedAtUtc { get; set; }
    public string? CollectedByUserId { get; set; }
    public byte[] RowVersion { get; set; } = [];
    public Order Order { get; set; } = null!;
    public ApplicationUser? CollectedByUser { get; set; }
}
