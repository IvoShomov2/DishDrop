using DishDrop.Data.Enums;

namespace DishDrop.Data.Entities;

public class CourierDeliveryArea
{
    public string CourierId { get; set; } = string.Empty;
    public int DeliveryAreaId { get; set; }
    public CourierProfile Courier { get; set; } = null!;
    public DeliveryArea DeliveryArea { get; set; } = null!;
}
