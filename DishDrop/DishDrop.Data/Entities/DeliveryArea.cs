using DishDrop.Data.Enums;

namespace DishDrop.Data.Entities;

public class DeliveryArea
{
    public int Id { get; set; }
    public int CityId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string NormalizedName { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public City City { get; set; } = null!;
}
