using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using DishDrop.Data.Enums;
using static DishDrop.Data.DataConstants.Order;
using static DishDrop.Data.DataConstants.Identity;

namespace DishDrop.Data.Entities
{
    [Table("Order")]
    public class Order
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(OrderNumberMaxLength)]
        public string OrderNumber { get; set; } = null!;

        [Required]
        [MaxLength(UserIdMaxLength)]
        public string CustomerId { get; set; } = null!;

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
        [MaxLength(CurrencyCodeMaxLength)]
        public string CurrencyCode { get; set; } = "EUR";

        [MaxLength(CustomerNoteMaxLength)]
        public string? CustomerNote { get; set; }

        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

        public DateTime? EstimatedReadyAtUtc { get; set; }

        public Guid CheckoutToken { get; set; }

        [Required]
        [MaxLength(CheckoutRequestHashMaxLength)]
        public string CheckoutRequestHash { get; set; } = null!;

        [Required]
        [MaxLength(RestaurantNameSnapshotMaxLength)]
        public string RestaurantNameSnapshot { get; set; } = null!;

        [Required]
        [MaxLength(PickupCitySnapshotMaxLength)]
        public string PickupCitySnapshot { get; set; } = null!;

        [Required]
        [MaxLength(PickupAddressSnapshotMaxLength)]
        public string PickupAddressSnapshot { get; set; } = null!;

        [Required]
        [MaxLength(PickupPhoneSnapshotMaxLength)]
        public string PickupPhoneSnapshot { get; set; } = null!;

        [Timestamp]
        public byte[] RowVersion { get; set; } = [];

        [ForeignKey(nameof(CustomerId))]
        public ApplicationUser Customer { get; set; } = null!;

        [ForeignKey(nameof(RestaurantId))]
        public Restaurant Restaurant { get; set; } = null!;

        public OrderDeliveryAddress? DeliveryAddress { get; set; }

        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();

        public ICollection<OrderStatusHistory> StatusHistory { get; set; } = new List<OrderStatusHistory>();

        public ICollection<Payment> Payments { get; set; } = new List<Payment>();

        public ICollection<Delivery> Deliveries { get; set; } = new List<Delivery>();

        public ICollection<Review> Reviews { get; set; } = new List<Review>();

        public ICollection<SupportTicket> SupportTickets { get; set; } = new List<SupportTicket>();

        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    }
}
