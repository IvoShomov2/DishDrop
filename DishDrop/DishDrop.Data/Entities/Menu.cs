using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DishDrop.Data.Enums;

namespace DishDrop.Data.Entities;

[Table("Menu")]
public class Menu
{
    [Key]
    public int Id { get; set; }
    public int RestaurantId { get; set; }
    [Required]
    [MaxLength(DataConstants.Menu.NameMaxLength)]
    public string Name { get; set; } = string.Empty;
    [MaxLength(DataConstants.Menu.DescriptionMaxLength)]
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
    public int DisplayOrder { get; set; }
    [ForeignKey(nameof(RestaurantId))]
    public Restaurant Restaurant { get; set; } = null!;
}
