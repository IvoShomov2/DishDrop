using DishDrop.Data.Enums;

namespace DishDrop.Data.Entities;

public class RestaurantDeliveryArea
{
    public int RestaurantId { get; set; }
    public int DeliveryAreaId { get; set; }
    public decimal DeliveryFee { get; set; }
    public bool IsActive { get; set; } = true;
    public Restaurant Restaurant { get; set; } = null!;
    public DeliveryArea DeliveryArea { get; set; } = null!;
}
