using DishDrop.Data.Enums;

namespace DishDrop.Data.Entities;

public class Restaurant
{
    public int Id { get; set; }
    public int CityId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string? ImagePath { get; set; }
    public ApprovalStatus ApprovalStatus { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsAcceptingOrders { get; set; }
    public decimal MinimumOrderAmount { get; set; }
    public int EstimatedPreparationMinutes { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public byte[] RowVersion { get; set; } = [];
    public City City { get; set; } = null!;
}
