using DishDrop.Data.Enums;

namespace DishDrop.Data.Entities;

public class AuditLog
{
    public int Id { get; set; }
    public string? ActorUserId { get; set; }
    public string Action { get; set; } = string.Empty;
    public string EntityType { get; set; } = string.Empty;
    public string EntityId { get; set; } = string.Empty;
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public string? Reason { get; set; }
    public string? CorrelationId { get; set; }
    public ApplicationUser? ActorUser { get; set; }
}
