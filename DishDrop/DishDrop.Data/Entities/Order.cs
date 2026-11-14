using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using DishDrop.Data.Enums;

namespace DishDrop.Data.Entities;

[Table("Order")]
public class Order
{
    [Key]
    public int Id { get; set; }
    [Required]
    [MaxLength(DataConstants.Order.OrderNumberMaxLength)]
    public string OrderNumber { get; set; } = string.Empty;
    [Required]
    [MaxLength(DataConstants.Identity.UserIdMaxLength)]
    public string CustomerId { get; set; } = string.Empty;
    public int RestaurantId { get; set; }
    public OrderStatus Status { get; set; }
    [Column(TypeName = DataConstants.Money.SqlType)]
    [Precision(DataConstants.Money.Precision, DataConstants.Money.Scale)]
    public decimal Subtotal { get; set; }
    [Column(TypeName = DataConstants.Money.SqlType)]
    [Precision(DataConstants.Money.Precision, DataConstants.Money.Scale)]
    public decimal DeliveryFee { get; set; }
    [Column(TypeName = DataConstants.Money.SqlType)]
    [Precision(DataConstants.Money.Precision, DataConstants.Money.Scale)]
    public decimal TotalAmount { get; set; }
    [Required]
    [MaxLength(DataConstants.Order.CurrencyCodeMaxLength)]
    public string CurrencyCode { get; set; } = "EUR";
    [MaxLength(DataConstants.Order.CustomerNoteMaxLength)]
    public string? CustomerNote { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime? EstimatedReadyAtUtc { get; set; }
    public Guid CheckoutToken { get; set; }
    [Required]
    [MaxLength(DataConstants.Order.CheckoutRequestHashMaxLength)]
    public string CheckoutRequestHash { get; set; } = string.Empty;
    [Required]
    [MaxLength(DataConstants.Order.RestaurantNameSnapshotMaxLength)]
    public string RestaurantNameSnapshot { get; set; } = string.Empty;
    [Required]
    [MaxLength(DataConstants.Order.PickupCitySnapshotMaxLength)]
    public string PickupCitySnapshot { get; set; } = string.Empty;
    [Required]
    [MaxLength(DataConstants.Order.PickupAddressSnapshotMaxLength)]
    public string PickupAddressSnapshot { get; set; } = string.Empty;
    [Required]
    [MaxLength(DataConstants.Order.PickupPhoneSnapshotMaxLength)]
    public string PickupPhoneSnapshot { get; set; } = string.Empty;
    [Timestamp]
    public byte[] RowVersion { get; set; } = [];
    [ForeignKey(nameof(CustomerId))]
    public ApplicationUser Customer { get; set; } = null!;
    [ForeignKey(nameof(RestaurantId))]
    public Restaurant Restaurant { get; set; } = null!;
}
