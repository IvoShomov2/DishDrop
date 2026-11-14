using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using DishDrop.Data.Enums;

namespace DishDrop.Data.Entities;

[Table("Restaurant")]
public class Restaurant
{
    [Key]
    public int Id { get; set; }
    public int CityId { get; set; }
    [Required]
    [MaxLength(DataConstants.Restaurant.NameMaxLength)]
    public string Name { get; set; } = string.Empty;
    [Required]
    [MaxLength(DataConstants.Restaurant.SlugMaxLength)]
    public string Slug { get; set; } = string.Empty;
    [Required]
    [MaxLength(DataConstants.Restaurant.DescriptionMaxLength)]
    public string Description { get; set; } = string.Empty;
    [Required]
    [MaxLength(DataConstants.Restaurant.AddressMaxLength)]
    public string Address { get; set; } = string.Empty;
    [Required]
    [MaxLength(DataConstants.Restaurant.PhoneNumberMaxLength)]
    public string PhoneNumber { get; set; } = string.Empty;
    [MaxLength(DataConstants.Restaurant.ImagePathMaxLength)]
    public string? ImagePath { get; set; }
    public ApprovalStatus ApprovalStatus { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsAcceptingOrders { get; set; }
    [Column(TypeName = DataConstants.Money.SqlType)]
    [Precision(DataConstants.Money.Precision, DataConstants.Money.Scale)]
    public decimal MinimumOrderAmount { get; set; }
    public int EstimatedPreparationMinutes { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    [Timestamp]
    public byte[] RowVersion { get; set; } = [];
    [ForeignKey(nameof(CityId))]
    public City City { get; set; } = null!;
}
