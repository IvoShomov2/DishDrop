using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static DishDrop.Data.DataConstants.Menu;

namespace DishDrop.Data.Entities
{
    [Table("Menu")]
    public class Menu
    {
        [Key]
        public int Id { get; set; }

        public int RestaurantId { get; set; }

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; } = null!;

        [MaxLength(DescriptionMaxLength)]
        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;

        public int DisplayOrder { get; set; }

        [ForeignKey(nameof(RestaurantId))]
        public Restaurant Restaurant { get; set; } = null!;

        public ICollection<MenuCategory> Categories { get; set; } = new List<MenuCategory>();
    }
}
