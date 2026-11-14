using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static DishDrop.Data.DataConstants.City;

namespace DishDrop.Data.Entities
{
    [Table("City")]
    public class City
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

        public ICollection<DeliveryArea> DeliveryAreas { get; set; } = new List<DeliveryArea>();

        public ICollection<Restaurant> Restaurants { get; set; } = new List<Restaurant>();
    }
}
