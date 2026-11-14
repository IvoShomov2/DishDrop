using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DishDrop.Data.Enums;

namespace DishDrop.Data.Entities;

[Table("MenuCategory")]
public class MenuCategory
{
    [Key]
    public int Id { get; set; }
    public int MenuId { get; set; }
    [Required]
    [MaxLength(DataConstants.MenuCategory.NameMaxLength)]
    public string Name { get; set; } = string.Empty;
    [Required]
    [MaxLength(DataConstants.MenuCategory.NormalizedNameMaxLength)]
    public string NormalizedName { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public int DisplayOrder { get; set; }
    [ForeignKey(nameof(MenuId))]
    public Menu Menu { get; set; } = null!;
}
