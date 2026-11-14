using DishDrop.Data.Enums;

namespace DishDrop.Data.Entities;

public class Dish
{
    public int Id { get; set; }
    public int MenuCategoryId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? ImagePath { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsAvailable { get; set; } = true;
    public int DisplayOrder { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public byte[] RowVersion { get; set; } = [];
    public MenuCategory MenuCategory { get; set; } = null!;
}
