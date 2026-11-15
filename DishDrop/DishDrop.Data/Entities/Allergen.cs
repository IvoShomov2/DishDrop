using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static DishDrop.Data.DataConstants.Allergen;

namespace DishDrop.Data.Entities
{
    [Table("Allergen")]
    public class Allergen
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(CodeMaxLength)]
        public string Code { get; set; } = null!;

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; } = null!;

        public ICollection<DishAllergen> DishAllergens { get; set; } = new List<DishAllergen>();

        public ICollection<DishOptionAllergen> DishOptionAllergens { get; set; } = new List<DishOptionAllergen>();
    }
}
