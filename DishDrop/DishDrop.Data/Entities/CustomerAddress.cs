using DishDrop.Data.Enums;

namespace DishDrop.Data.Entities;

public class CustomerAddress
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public int DeliveryAreaId { get; set; }
    public string Label { get; set; } = string.Empty;
    public string StreetAddress { get; set; } = string.Empty;
    public string? Building { get; set; }
    public string? Entrance { get; set; }
    public string? Floor { get; set; }
    public string? Apartment { get; set; }
    public string? DeliveryInstructions { get; set; }
    public bool IsDefault { get; set; }
    public ApplicationUser User { get; set; } = null!;
    public DeliveryArea DeliveryArea { get; set; } = null!;
}
