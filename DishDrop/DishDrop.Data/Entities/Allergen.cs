using DishDrop.Data.Enums;

namespace DishDrop.Data.Entities;

public class Allergen
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}
