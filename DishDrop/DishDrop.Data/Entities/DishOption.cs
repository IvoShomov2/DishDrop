using DishDrop.Data.Enums;

namespace DishDrop.Data.Entities;

public class DishOption
{
    public int Id { get; set; }
    public int DishOptionGroupId { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal AdditionalPrice { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsAvailable { get; set; } = true;
    public int DisplayOrder { get; set; }
    public DishOptionGroup DishOptionGroup { get; set; } = null!;
}
