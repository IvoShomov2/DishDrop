using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using DishDrop.Data.Enums;

namespace DishDrop.Data.Entities;

[Table("DishVariant")]
public class DishVariant
{
    [Key]
    public int Id { get; set; }
    public int DishId { get; set; }
    [Required]
    [MaxLength(DataConstants.DishVariant.NameMaxLength)]
    public string Name { get; set; } = string.Empty;
    [Required]
    [MaxLength(DataConstants.DishVariant.NormalizedNameMaxLength)]
    public string NormalizedName { get; set; } = string.Empty;
    [Column(TypeName = DataConstants.Money.SqlType)]
    [Precision(DataConstants.Money.Precision, DataConstants.Money.Scale)]
    public decimal Price { get; set; }
    [Column(TypeName = DataConstants.DishVariant.PortionQuantitySqlType)]
    [Precision(DataConstants.DishVariant.PortionQuantityPrecision, DataConstants.DishVariant.PortionQuantityScale)]
    public decimal? PortionQuantity { get; set; }
    public PortionUnit? PortionUnit { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsAvailable { get; set; } = true;
    public int DisplayOrder { get; set; }
    [ForeignKey(nameof(DishId))]
    public Dish Dish { get; set; } = null!;
}
