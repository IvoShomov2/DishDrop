using DishDrop.Data.Enums;

namespace DishDrop.Data.Entities;

public class ReviewReply
{
    public int ReviewId { get; set; }
    public string AuthorUserId { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAtUtc { get; set; } = DateTime.UtcNow;
    public ModerationStatus ModerationStatus { get; set; }
    public Review Review { get; set; } = null!;
    public ApplicationUser AuthorUser { get; set; } = null!;
}
