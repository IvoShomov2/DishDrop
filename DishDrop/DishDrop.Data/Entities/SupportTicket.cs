using DishDrop.Data.Enums;

namespace DishDrop.Data.Entities;

public class SupportTicket
{
    public int Id { get; set; }
    public string CreatedByUserId { get; set; } = string.Empty;
    public int? OrderId { get; set; }
    public SupportCategory Category { get; set; }
    public string Subject { get; set; } = string.Empty;
    public SupportStatus Status { get; set; }
    public string? AssignedAdminId { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAtUtc { get; set; } = DateTime.UtcNow;
    public byte[] RowVersion { get; set; } = [];
    public ApplicationUser CreatedByUser { get; set; } = null!;
    public Order? Order { get; set; }
    public ApplicationUser? AssignedAdmin { get; set; }
}
