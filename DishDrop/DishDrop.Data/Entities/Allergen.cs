using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DishDrop.Data.Enums;

namespace DishDrop.Data.Entities;

[Table("Allergen")]
public class Allergen
{
    [Key]
    public int Id { get; set; }
    [Required]
    [MaxLength(DataConstants.Allergen.CodeMaxLength)]
    public string Code { get; set; } = string.Empty;
    [Required]
    [MaxLength(DataConstants.Allergen.NameMaxLength)]
    public string Name { get; set; } = string.Empty;
}
