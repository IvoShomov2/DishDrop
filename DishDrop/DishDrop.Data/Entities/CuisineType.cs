using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static DishDrop.Data.DataConstants.CuisineType;

namespace DishDrop.Data.Entities
{
    [Table("CuisineType")]
    public class CuisineType
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(NormalizedNameMaxLength)]
        public string NormalizedName { get; set; } = null!;

        public bool IsActive { get; set; } = true;

        public ICollection<RestaurantCuisineType> RestaurantCuisineTypes { get; set; } = new List<RestaurantCuisineType>();
    }
}
