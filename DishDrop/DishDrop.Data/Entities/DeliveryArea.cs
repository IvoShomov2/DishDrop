using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static DishDrop.Data.DataConstants.DeliveryArea;

namespace DishDrop.Data.Entities
{
    [Table("DeliveryArea")]
    public class DeliveryArea
    {
        [Key]
        public int Id { get; set; }

        public int CityId { get; set; }

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(NormalizedNameMaxLength)]
        public string NormalizedName { get; set; } = null!;

        public bool IsActive { get; set; } = true;

        [ForeignKey(nameof(CityId))]
        public City City { get; set; } = null!;

        public ICollection<CustomerAddress> CustomerAddresses { get; set; } = new List<CustomerAddress>();

        public ICollection<RestaurantDeliveryArea> RestaurantDeliveryAreas { get; set; } = new List<RestaurantDeliveryArea>();

        public ICollection<CourierDeliveryArea> CourierDeliveryAreas { get; set; } = new List<CourierDeliveryArea>();

        public ICollection<OrderDeliveryAddress> OrderDeliveryAddresses { get; set; } = new List<OrderDeliveryAddress>();

        public ICollection<Delivery> Deliveries { get; set; } = new List<Delivery>();
    }
}
