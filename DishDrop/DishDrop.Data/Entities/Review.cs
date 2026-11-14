using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DishDrop.Data.Enums;
using static DishDrop.Data.DataConstants.Review;

namespace DishDrop.Data.Entities
{
    [Table("Review")]
    public class Review
    {
        [Key]
        public int Id { get; set; }

        public int OrderId { get; set; }

        public int RestaurantRating { get; set; }

        public int? DeliveryRating { get; set; }

        [MaxLength(CommentMaxLength)]
        public string? Comment { get; set; }

        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAtUtc { get; set; } = DateTime.UtcNow;

        public ModerationStatus ModerationStatus { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; } = [];

        [ForeignKey(nameof(OrderId))]
        public Order Order { get; set; } = null!;

        public ReviewReply? Reply { get; set; }
    }
}
