using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static DishDrop.Data.DataConstants.Dish;

namespace DishDrop.Data.Entities
{
    [Table("Dish")]
    public class Dish
    {
        [Key]
        public int Id { get; set; }

        public int MenuCategoryId { get; set; }

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(DescriptionMaxLength)]
        public string Description { get; set; } = null!;

        [MaxLength(ImagePathMaxLength)]
        public string? ImagePath { get; set; }

        public bool IsActive { get; set; } = true;

        public bool IsAvailable { get; set; } = true;

        public int DisplayOrder { get; set; }

        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

        [Timestamp]
        public byte[] RowVersion { get; set; } = [];

        [ForeignKey(nameof(MenuCategoryId))]
        public MenuCategory MenuCategory { get; set; } = null!;

        public ICollection<DishVariant> Variants { get; set; } = new List<DishVariant>();

        public ICollection<DishOptionGroup> OptionGroups { get; set; } = new List<DishOptionGroup>();

        public ICollection<DishAllergen> DishAllergens { get; set; } = new List<DishAllergen>();
    }
}
