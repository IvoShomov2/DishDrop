using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DishDrop.Data.Enums;

namespace DishDrop.Data.Entities;

[Table("CuisineType")]
public class CuisineType
{
    [Key]
    public int Id { get; set; }
    [Required]
    [MaxLength(DataConstants.CuisineType.NameMaxLength)]
    public string Name { get; set; } = string.Empty;
    [Required]
    [MaxLength(DataConstants.CuisineType.NormalizedNameMaxLength)]
    public string NormalizedName { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
}
