using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static DishDrop.Data.DataConstants.OrderDeliveryAddress;

namespace DishDrop.Data.Entities
{
    [Table("OrderDeliveryAddress")]
    public class OrderDeliveryAddress
    {
        [Key]
        public int OrderId { get; set; }

        public int DeliveryAreaId { get; set; }

        [Required]
        [MaxLength(RecipientNameMaxLength)]
        public string RecipientName { get; set; } = null!;

        [Required]
        [MaxLength(PhoneNumberMaxLength)]
        public string PhoneNumber { get; set; } = null!;

        [Required]
        [MaxLength(CityNameMaxLength)]
        public string CityName { get; set; } = null!;

        [Required]
        [MaxLength(AreaNameMaxLength)]
        public string AreaName { get; set; } = null!;

        [Required]
        [MaxLength(StreetAddressMaxLength)]
        public string StreetAddress { get; set; } = null!;

        [MaxLength(BuildingMaxLength)]
        public string? Building { get; set; }

        [MaxLength(EntranceMaxLength)]
        public string? Entrance { get; set; }

        [MaxLength(FloorMaxLength)]
        public string? Floor { get; set; }

        [MaxLength(ApartmentMaxLength)]
        public string? Apartment { get; set; }

        [MaxLength(DeliveryInstructionsMaxLength)]
        public string? DeliveryInstructions { get; set; }

        [ForeignKey(nameof(OrderId))]
        public Order Order { get; set; } = null!;

        [ForeignKey(nameof(DeliveryAreaId))]
        public DeliveryArea DeliveryArea { get; set; } = null!;
    }
}
