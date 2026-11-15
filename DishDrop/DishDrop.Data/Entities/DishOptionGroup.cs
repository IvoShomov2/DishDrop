using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static DishDrop.Data.DataConstants.DishOptionGroup;

namespace DishDrop.Data.Entities
{
    [Table("DishOptionGroup")]
    public class DishOptionGroup
    {
        [Key]
        public int Id { get; set; }

        public int DishId { get; set; }

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; } = null!;

        public int MinSelections { get; set; }

        public int MaxSelections { get; set; }

        public bool IsActive { get; set; } = true;

        public int DisplayOrder { get; set; }

        [ForeignKey(nameof(DishId))]
        public Dish Dish { get; set; } = null!;

        public ICollection<DishOption> Options { get; set; } = new List<DishOption>();
    }
}
