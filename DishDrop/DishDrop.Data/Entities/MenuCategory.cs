using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static DishDrop.Data.DataConstants.MenuCategory;

namespace DishDrop.Data.Entities
{
    [Table("MenuCategory")]
    public class MenuCategory
    {
        [Key]
        public int Id { get; set; }

        public int MenuId { get; set; }

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(NormalizedNameMaxLength)]
        public string NormalizedName { get; set; } = null!;

        public bool IsActive { get; set; } = true;

        public int DisplayOrder { get; set; }

        [ForeignKey(nameof(MenuId))]
        public Menu Menu { get; set; } = null!;

        public ICollection<Dish> Dishes { get; set; } = new List<Dish>();
    }
}
