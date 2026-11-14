using DishDrop.Data.Enums;

namespace DishDrop.Data.Entities;

public class DishVariant
{
    public int Id { get; set; }
    public int DishId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string NormalizedName { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public decimal? PortionQuantity { get; set; }
    public PortionUnit? PortionUnit { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsAvailable { get; set; } = true;
    public int DisplayOrder { get; set; }
    public Dish Dish { get; set; } = null!;
}
