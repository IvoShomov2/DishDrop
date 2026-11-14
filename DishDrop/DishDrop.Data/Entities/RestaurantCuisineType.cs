using System.ComponentModel.DataAnnotations.Schema;

namespace DishDrop.Data.Entities
{
    [Table("RestaurantCuisineType")]
    public class RestaurantCuisineType
    {
        public int RestaurantId { get; set; }

        public int CuisineTypeId { get; set; }

        [ForeignKey(nameof(RestaurantId))]
        public Restaurant Restaurant { get; set; } = null!;

        [ForeignKey(nameof(CuisineTypeId))]
        public CuisineType CuisineType { get; set; } = null!;
    }
}
