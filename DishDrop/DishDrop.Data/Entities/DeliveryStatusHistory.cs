using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DishDrop.Data.Enums;

namespace DishDrop.Data.Entities;

[Table("DeliveryStatusHistory")]
public class DeliveryStatusHistory
{
    [Key]
    public int Id { get; set; }
    public int DeliveryId { get; set; }
    public DeliveryStatus? FromStatus { get; set; }
    public DeliveryStatus ToStatus { get; set; }
    [MaxLength(DataConstants.Identity.UserIdMaxLength)]
    public string? ChangedByUserId { get; set; }
    public DateTime ChangedAtUtc { get; set; }
    [Required]
    [MaxLength(DataConstants.DeliveryStatusHistory.ReasonMaxLength)]
    public string Reason { get; set; } = string.Empty;
    [ForeignKey(nameof(DeliveryId))]
    public Delivery Delivery { get; set; } = null!;
    [ForeignKey(nameof(ChangedByUserId))]
    public ApplicationUser? ChangedByUser { get; set; }
}
