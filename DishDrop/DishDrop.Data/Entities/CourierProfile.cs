using DishDrop.Data.Enums;

namespace DishDrop.Data.Entities;

public class CourierProfile
{
    public string UserId { get; set; } = string.Empty;
    public ApprovalStatus ApprovalStatus { get; set; }
    public VehicleType VehicleType { get; set; }
    public bool IsAvailable { get; set; } = true;
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime? ApprovedAtUtc { get; set; }
    public byte[] RowVersion { get; set; } = [];
    public ApplicationUser User { get; set; } = null!;
}
