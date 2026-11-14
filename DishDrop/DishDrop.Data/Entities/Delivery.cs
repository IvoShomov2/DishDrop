using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DishDrop.Data.Enums;

namespace DishDrop.Data.Entities;

[Table("Delivery")]
public class Delivery
{
    [Key]
    public int Id { get; set; }
    public int OrderId { get; set; }
    [MaxLength(DataConstants.Identity.UserIdMaxLength)]
    public string? CourierId { get; set; }
    public int DeliveryAreaId { get; set; }
    public DeliveryStatus Status { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime? AssignedAtUtc { get; set; }
    public DateTime? PickedUpAtUtc { get; set; }
    public DateTime? DeliveredAtUtc { get; set; }
    [Timestamp]
    public byte[] RowVersion { get; set; } = [];
    [ForeignKey(nameof(OrderId))]
    public Order Order { get; set; } = null!;
    [ForeignKey(nameof(CourierId))]
    public CourierProfile? Courier { get; set; }
    [ForeignKey(nameof(DeliveryAreaId))]
    public DeliveryArea DeliveryArea { get; set; } = null!;
}
