using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static DishDrop.Data.DataConstants.Identity;

namespace DishDrop.Data.Entities
{
    [Table("CourierDeliveryArea")]
    public class CourierDeliveryArea
    {
        [Required]
        [MaxLength(UserIdMaxLength)]
        public string CourierId { get; set; } = null!;

        public int DeliveryAreaId { get; set; }

        [ForeignKey(nameof(CourierId))]
        public CourierProfile Courier { get; set; } = null!;

        [ForeignKey(nameof(DeliveryAreaId))]
        public DeliveryArea DeliveryArea { get; set; } = null!;
    }
}
