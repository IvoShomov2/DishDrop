using DishDrop.Data.Enums;

namespace DishDrop.Data.Entities;

public class RestaurantApprovalHistory
{
    public int Id { get; set; }
    public int RestaurantId { get; set; }
    public ApprovalStatus? FromStatus { get; set; }
    public ApprovalStatus ToStatus { get; set; }
    public string ChangedByUserId { get; set; } = string.Empty;
    public DateTime ChangedAtUtc { get; set; }
    public string Reason { get; set; } = string.Empty;
    public Restaurant Restaurant { get; set; } = null!;
    public ApplicationUser ChangedByUser { get; set; } = null!;
}
