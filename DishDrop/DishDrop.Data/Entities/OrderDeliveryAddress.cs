using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DishDrop.Data.Enums;

namespace DishDrop.Data.Entities;

[Table("OrderDeliveryAddress")]
public class OrderDeliveryAddress
{
    [Key]
    public int OrderId { get; set; }
    public int DeliveryAreaId { get; set; }
    [Required]
    [MaxLength(DataConstants.OrderDeliveryAddress.RecipientNameMaxLength)]
    public string RecipientName { get; set; } = string.Empty;
    [Required]
    [MaxLength(DataConstants.OrderDeliveryAddress.PhoneNumberMaxLength)]
    public string PhoneNumber { get; set; } = string.Empty;
    [Required]
    [MaxLength(DataConstants.OrderDeliveryAddress.CityNameMaxLength)]
    public string CityName { get; set; } = string.Empty;
    [Required]
    [MaxLength(DataConstants.OrderDeliveryAddress.AreaNameMaxLength)]
    public string AreaName { get; set; } = string.Empty;
    [Required]
    [MaxLength(DataConstants.OrderDeliveryAddress.StreetAddressMaxLength)]
    public string StreetAddress { get; set; } = string.Empty;
    [MaxLength(DataConstants.OrderDeliveryAddress.BuildingMaxLength)]
    public string? Building { get; set; }
    [MaxLength(DataConstants.OrderDeliveryAddress.EntranceMaxLength)]
    public string? Entrance { get; set; }
    [MaxLength(DataConstants.OrderDeliveryAddress.FloorMaxLength)]
    public string? Floor { get; set; }
    [MaxLength(DataConstants.OrderDeliveryAddress.ApartmentMaxLength)]
    public string? Apartment { get; set; }
    [MaxLength(DataConstants.OrderDeliveryAddress.DeliveryInstructionsMaxLength)]
    public string? DeliveryInstructions { get; set; }
    [ForeignKey(nameof(OrderId))]
    public Order Order { get; set; } = null!;
    [ForeignKey(nameof(DeliveryAreaId))]
    public DeliveryArea DeliveryArea { get; set; } = null!;
}
