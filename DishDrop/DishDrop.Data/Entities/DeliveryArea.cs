using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DishDrop.Data.Enums;

namespace DishDrop.Data.Entities;

[Table("DeliveryArea")]
public class DeliveryArea
{
    [Key]
    public int Id { get; set; }
    public int CityId { get; set; }
    [Required]
    [MaxLength(DataConstants.DeliveryArea.NameMaxLength)]
    public string Name { get; set; } = string.Empty;
    [Required]
    [MaxLength(DataConstants.DeliveryArea.NormalizedNameMaxLength)]
    public string NormalizedName { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    [ForeignKey(nameof(CityId))]
    public City City { get; set; } = null!;
}
