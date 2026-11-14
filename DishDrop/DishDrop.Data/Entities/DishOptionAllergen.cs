using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DishDrop.Data.Enums;

namespace DishDrop.Data.Entities;

[Table("DishOptionAllergen")]
public class DishOptionAllergen
{
    public int DishOptionId { get; set; }
    public int AllergenId { get; set; }
    [ForeignKey(nameof(DishOptionId))]
    public DishOption DishOption { get; set; } = null!;
    [ForeignKey(nameof(AllergenId))]
    public Allergen Allergen { get; set; } = null!;
}
