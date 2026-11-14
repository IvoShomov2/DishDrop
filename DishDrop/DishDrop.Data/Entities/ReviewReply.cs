using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DishDrop.Data.Enums;

namespace DishDrop.Data.Entities;

[Table("ReviewReply")]
public class ReviewReply
{
    [Key]
    public int ReviewId { get; set; }
    [Required]
    [MaxLength(DataConstants.Identity.UserIdMaxLength)]
    public string AuthorUserId { get; set; } = string.Empty;
    [Required]
    [MaxLength(DataConstants.ReviewReply.TextMaxLength)]
    public string Text { get; set; } = string.Empty;
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAtUtc { get; set; } = DateTime.UtcNow;
    public ModerationStatus ModerationStatus { get; set; }
    [ForeignKey(nameof(ReviewId))]
    public Review Review { get; set; } = null!;
    [ForeignKey(nameof(AuthorUserId))]
    public ApplicationUser AuthorUser { get; set; } = null!;
}
