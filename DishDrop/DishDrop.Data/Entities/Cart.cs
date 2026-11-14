using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static DishDrop.Data.DataConstants.Identity;

namespace DishDrop.Data.Entities
{
    [Table("Cart")]
    public class Cart
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(UserIdMaxLength)]
        public string UserId { get; set; } = null!;

        public int? RestaurantId { get; set; }

        public DateTime UpdatedAtUtc { get; set; } = DateTime.UtcNow;

        [Timestamp]
        public byte[] RowVersion { get; set; } = [];

        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; } = null!;

        [ForeignKey(nameof(RestaurantId))]
        public Restaurant? Restaurant { get; set; }

        public ICollection<CartItem> Items { get; set; } = new List<CartItem>();
    }
}
