using DishDrop.Data.Enums;

namespace DishDrop.Data.Entities;

public class DishAllergen
{
    public int DishId { get; set; }
    public int AllergenId { get; set; }
    public Dish Dish { get; set; } = null!;
    public Allergen Allergen { get; set; } = null!;
}
