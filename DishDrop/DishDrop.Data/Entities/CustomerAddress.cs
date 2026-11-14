using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DishDrop.Data.Enums;

namespace DishDrop.Data.Entities;

[Table("CustomerAddress")]
public class CustomerAddress
{
    [Key]
    public int Id { get; set; }
    [Required]
    [MaxLength(DataConstants.Identity.UserIdMaxLength)]
    public string UserId { get; set; } = string.Empty;
    public int DeliveryAreaId { get; set; }
    [Required]
    [MaxLength(DataConstants.CustomerAddress.LabelMaxLength)]
    public string Label { get; set; } = string.Empty;
    [Required]
    [MaxLength(DataConstants.CustomerAddress.StreetAddressMaxLength)]
    public string StreetAddress { get; set; } = string.Empty;
    [MaxLength(DataConstants.CustomerAddress.BuildingMaxLength)]
    public string? Building { get; set; }
    [MaxLength(DataConstants.CustomerAddress.EntranceMaxLength)]
    public string? Entrance { get; set; }
    [MaxLength(DataConstants.CustomerAddress.FloorMaxLength)]
    public string? Floor { get; set; }
    [MaxLength(DataConstants.CustomerAddress.ApartmentMaxLength)]
    public string? Apartment { get; set; }
    [MaxLength(DataConstants.CustomerAddress.DeliveryInstructionsMaxLength)]
    public string? DeliveryInstructions { get; set; }
    public bool IsDefault { get; set; }
    [ForeignKey(nameof(UserId))]
    public ApplicationUser User { get; set; } = null!;
    [ForeignKey(nameof(DeliveryAreaId))]
    public DeliveryArea DeliveryArea { get; set; } = null!;
}
