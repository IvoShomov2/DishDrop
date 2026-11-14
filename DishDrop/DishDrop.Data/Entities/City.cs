using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DishDrop.Data.Enums;

namespace DishDrop.Data.Entities;

[Table("City")]
public class City
{
    [Key]
    public int Id { get; set; }
    [Required]
    [MaxLength(DataConstants.City.NameMaxLength)]
    public string Name { get; set; } = string.Empty;
    [Required]
    [MaxLength(DataConstants.City.NormalizedNameMaxLength)]
    public string NormalizedName { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
}
