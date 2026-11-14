using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using DishDrop.Data.Enums;

namespace DishDrop.Data.Entities;

[Table("OrderItem")]
public class OrderItem
{
    [Key]
    public int Id { get; set; }
    public int OrderId { get; set; }
    public int DishVariantId { get; set; }
    [Required]
    [MaxLength(DataConstants.OrderItem.DishNameMaxLength)]
    public string DishName { get; set; } = string.Empty;
    [Required]
    [MaxLength(DataConstants.OrderItem.VariantNameMaxLength)]
    public string VariantName { get; set; } = string.Empty;
    [Column(TypeName = DataConstants.Money.SqlType)]
    [Precision(DataConstants.Money.Precision, DataConstants.Money.Scale)]
    public decimal BaseUnitPrice { get; set; }
    [Column(TypeName = DataConstants.Money.SqlType)]
    [Precision(DataConstants.Money.Precision, DataConstants.Money.Scale)]
    public decimal OptionsUnitPrice { get; set; }
    public int Quantity { get; set; }
    [Column(TypeName = DataConstants.Money.SqlType)]
    [Precision(DataConstants.Money.Precision, DataConstants.Money.Scale)]
    public decimal LineTotal { get; set; }
    [MaxLength(DataConstants.OrderItem.SpecialInstructionsMaxLength)]
    public string? SpecialInstructions { get; set; }
    [ForeignKey(nameof(OrderId))]
    public Order Order { get; set; } = null!;
    [ForeignKey(nameof(DishVariantId))]
    public DishVariant DishVariant { get; set; } = null!;
}
