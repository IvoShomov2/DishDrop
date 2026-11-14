using DishDrop.Data.Enums;

namespace DishDrop.Data.Entities;

public class DishOptionGroup
{
    public int Id { get; set; }
    public int DishId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int MinSelections { get; set; }
    public int MaxSelections { get; set; }
    public bool IsActive { get; set; } = true;
    public int DisplayOrder { get; set; }
    public Dish Dish { get; set; } = null!;
}
