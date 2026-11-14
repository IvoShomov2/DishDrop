using DishDrop.Data.Enums;

namespace DishDrop.Data.Entities;

public class OrderDeliveryAddress
{
    public int OrderId { get; set; }
    public int DeliveryAreaId { get; set; }
    public string RecipientName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string CityName { get; set; } = string.Empty;
    public string AreaName { get; set; } = string.Empty;
    public string StreetAddress { get; set; } = string.Empty;
    public string? Building { get; set; }
    public string? Entrance { get; set; }
    public string? Floor { get; set; }
    public string? Apartment { get; set; }
    public string? DeliveryInstructions { get; set; }
    public Order Order { get; set; } = null!;
    public DeliveryArea DeliveryArea { get; set; } = null!;
}
