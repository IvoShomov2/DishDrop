using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DishDrop.Data.Enums;
using static DishDrop.Data.DataConstants.ReviewReply;
using static DishDrop.Data.DataConstants.Identity;

namespace DishDrop.Data.Entities
{
    [Table("ReviewReply")]
    public class ReviewReply
    {
        [Key]
        public int ReviewId { get; set; }

        [Required]
        [MaxLength(UserIdMaxLength)]
        public string AuthorUserId { get; set; } = null!;

        [Required]
        [MaxLength(TextMaxLength)]
        public string Text { get; set; } = null!;

        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAtUtc { get; set; } = DateTime.UtcNow;

        public ModerationStatus ModerationStatus { get; set; }

        [ForeignKey(nameof(ReviewId))]
        public Review Review { get; set; } = null!;

        [ForeignKey(nameof(AuthorUserId))]
        public ApplicationUser AuthorUser { get; set; } = null!;
    }
}
