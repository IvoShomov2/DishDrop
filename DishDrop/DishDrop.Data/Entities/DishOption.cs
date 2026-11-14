using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using DishDrop.Data.Enums;

namespace DishDrop.Data.Entities;

[Table("DishOption")]
public class DishOption
{
    [Key]
    public int Id { get; set; }
    public int DishOptionGroupId { get; set; }
    [Required]
    [MaxLength(DataConstants.DishOption.NameMaxLength)]
    public string Name { get; set; } = string.Empty;
    [Column(TypeName = DataConstants.Money.SqlType)]
    [Precision(DataConstants.Money.Precision, DataConstants.Money.Scale)]
    public decimal AdditionalPrice { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsAvailable { get; set; } = true;
    public int DisplayOrder { get; set; }
    [ForeignKey(nameof(DishOptionGroupId))]
    public DishOptionGroup DishOptionGroup { get; set; } = null!;
}
