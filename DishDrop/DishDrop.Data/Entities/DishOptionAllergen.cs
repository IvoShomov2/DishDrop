using DishDrop.Data.Enums;

namespace DishDrop.Data.Entities;

public class DishOptionAllergen
{
    public int DishOptionId { get; set; }
    public int AllergenId { get; set; }
    public DishOption DishOption { get; set; } = null!;
    public Allergen Allergen { get; set; } = null!;
}
