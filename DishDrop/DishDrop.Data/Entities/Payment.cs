using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using DishDrop.Data.Enums;
using static DishDrop.Data.DataConstants.Payment;
using static DishDrop.Data.DataConstants.Identity;

namespace DishDrop.Data.Entities
{
    [Table("Payment")]
    public class Payment
    {
        [Key]
        public int Id { get; set; }

        public int OrderId { get; set; }

        public PaymentMethod Method { get; set; }

        public PaymentStatus Status { get; set; }

        [Column(TypeName = DataConstants.Money.SqlType)]
        [Precision(DataConstants.Money.Precision, DataConstants.Money.Scale)]
        public decimal Amount { get; set; }

        [Required]
        [MaxLength(CurrencyCodeMaxLength)]
        public string CurrencyCode { get; set; } = "EUR";

        public DateTime? CollectedAtUtc { get; set; }

        [MaxLength(UserIdMaxLength)]
        public string? CollectedByUserId { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; } = [];

        [ForeignKey(nameof(OrderId))]
        public Order Order { get; set; } = null!;

        [ForeignKey(nameof(CollectedByUserId))]
        public ApplicationUser? CollectedByUser { get; set; }
    }
}
