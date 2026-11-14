using DishDrop.Data.Enums;

namespace DishDrop.Data.Entities;

public class MenuCategory
{
    public int Id { get; set; }
    public int MenuId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string NormalizedName { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public int DisplayOrder { get; set; }
    public Menu Menu { get; set; } = null!;
}
