using System.ComponentModel.DataAnnotations.Schema;

namespace DishDrop.Data.Entities
{
    [Table("DishAllergen")]
    public class DishAllergen
    {
        public int DishId { get; set; }

        public int AllergenId { get; set; }

        [ForeignKey(nameof(DishId))]
        public Dish Dish { get; set; } = null!;

        [ForeignKey(nameof(AllergenId))]
        public Allergen Allergen { get; set; } = null!;
    }
}
