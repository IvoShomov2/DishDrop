using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DishDrop.Data.Enums;

namespace DishDrop.Data.Entities;

[Table("Dish")]
public class Dish
{
    [Key]
    public int Id { get; set; }
    public int MenuCategoryId { get; set; }
    [Required]
    [MaxLength(DataConstants.Dish.NameMaxLength)]
    public string Name { get; set; } = string.Empty;
    [Required]
    [MaxLength(DataConstants.Dish.DescriptionMaxLength)]
    public string Description { get; set; } = string.Empty;
    [MaxLength(DataConstants.Dish.ImagePathMaxLength)]
    public string? ImagePath { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsAvailable { get; set; } = true;
    public int DisplayOrder { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    [Timestamp]
    public byte[] RowVersion { get; set; } = [];
    [ForeignKey(nameof(MenuCategoryId))]
    public MenuCategory MenuCategory { get; set; } = null!;
}
