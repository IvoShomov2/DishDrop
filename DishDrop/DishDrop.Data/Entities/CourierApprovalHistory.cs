using DishDrop.Data.Enums;

namespace DishDrop.Data.Entities;

public class CourierApprovalHistory
{
    public int Id { get; set; }
    public string CourierId { get; set; } = string.Empty;
    public ApprovalStatus? FromStatus { get; set; }
    public ApprovalStatus ToStatus { get; set; }
    public string ChangedByUserId { get; set; } = string.Empty;
    public DateTime ChangedAtUtc { get; set; }
    public string Reason { get; set; } = string.Empty;
    public CourierProfile Courier { get; set; } = null!;
    public ApplicationUser ChangedByUser { get; set; } = null!;
}
