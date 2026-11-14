using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static DishDrop.Data.DataConstants.CustomerAddress;
using static DishDrop.Data.DataConstants.Identity;

namespace DishDrop.Data.Entities
{
    [Table("CustomerAddress")]
    public class CustomerAddress
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(UserIdMaxLength)]
        public string UserId { get; set; } = null!;

        public int DeliveryAreaId { get; set; }

        [Required]
        [MaxLength(LabelMaxLength)]
        public string Label { get; set; } = null!;

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

        public bool IsDefault { get; set; }

        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; } = null!;

        [ForeignKey(nameof(DeliveryAreaId))]
        public DeliveryArea DeliveryArea { get; set; } = null!;
    }
}
