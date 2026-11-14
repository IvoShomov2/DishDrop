using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using DishDrop.Data.Enums;

namespace DishDrop.Data.Entities;

[Table("OrderItemOption")]
public class OrderItemOption
{
    [Key]
    public int Id { get; set; }
    public int OrderItemId { get; set; }
    public int DishOptionId { get; set; }
    [Required]
    [MaxLength(DataConstants.OrderItemOption.NameSnapshotMaxLength)]
    public string NameSnapshot { get; set; } = string.Empty;
    [Column(TypeName = DataConstants.Money.SqlType)]
    [Precision(DataConstants.Money.Precision, DataConstants.Money.Scale)]
    public decimal AdditionalPrice { get; set; }
    [ForeignKey(nameof(OrderItemId))]
    public OrderItem OrderItem { get; set; } = null!;
    [ForeignKey(nameof(DishOptionId))]
    public DishOption DishOption { get; set; } = null!;
}
