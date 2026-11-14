using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DishDrop.Data.Enums;

namespace DishDrop.Data.Entities;

[Table("CourierDeliveryArea")]
public class CourierDeliveryArea
{
    [Required]
    [MaxLength(DataConstants.Identity.UserIdMaxLength)]
    public string CourierId { get; set; } = string.Empty;
    public int DeliveryAreaId { get; set; }
    [ForeignKey(nameof(CourierId))]
    public CourierProfile Courier { get; set; } = null!;
    [ForeignKey(nameof(DeliveryAreaId))]
    public DeliveryArea DeliveryArea { get; set; } = null!;
}
