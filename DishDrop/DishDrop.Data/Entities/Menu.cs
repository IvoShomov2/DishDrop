using DishDrop.Data.Enums;

namespace DishDrop.Data.Entities;

public class Menu
{
    public int Id { get; set; }
    public int RestaurantId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
    public int DisplayOrder { get; set; }
    public Restaurant Restaurant { get; set; } = null!;
}
