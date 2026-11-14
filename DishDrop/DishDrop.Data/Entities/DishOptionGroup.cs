using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DishDrop.Data.Enums;

namespace DishDrop.Data.Entities;

[Table("DishOptionGroup")]
public class DishOptionGroup
{
    [Key]
    public int Id { get; set; }
    public int DishId { get; set; }
    [Required]
    [MaxLength(DataConstants.DishOptionGroup.NameMaxLength)]
    public string Name { get; set; } = string.Empty;
    public int MinSelections { get; set; }
    public int MaxSelections { get; set; }
    public bool IsActive { get; set; } = true;
    public int DisplayOrder { get; set; }
    [ForeignKey(nameof(DishId))]
    public Dish Dish { get; set; } = null!;
}
