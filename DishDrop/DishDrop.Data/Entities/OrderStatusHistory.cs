using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DishDrop.Data.Enums;
using static DishDrop.Data.DataConstants.OrderStatusHistory;
using static DishDrop.Data.DataConstants.Identity;

namespace DishDrop.Data.Entities
{
    [Table("OrderStatusHistory")]
    public class OrderStatusHistory
    {
        [Key]
        public int Id { get; set; }

        public int OrderId { get; set; }

        public OrderStatus? FromStatus { get; set; }

        public OrderStatus ToStatus { get; set; }

        [MaxLength(UserIdMaxLength)]
        public string? ChangedByUserId { get; set; }

        public DateTime ChangedAtUtc { get; set; }

        [Required]
        [MaxLength(ReasonMaxLength)]
        public string Reason { get; set; } = null!;

        [ForeignKey(nameof(OrderId))]
        public Order Order { get; set; } = null!;

        [ForeignKey(nameof(ChangedByUserId))]
        public ApplicationUser? ChangedByUser { get; set; }
    }
}
