using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DishDrop.Data.Enums;
using Microsoft.AspNetCore.Identity;

namespace DishDrop.Data.Entities;

[Table("AspNetUsers")]
public class ApplicationUser : IdentityUser
{
    [Required]
    [MaxLength(DataConstants.ApplicationUser.FirstNameMaxLength)]
    public string FirstName { get; set; } = string.Empty;
    [Required]
    [MaxLength(DataConstants.ApplicationUser.LastNameMaxLength)]
    public string LastName { get; set; } = string.Empty;
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public bool IsActive { get; set; } = true;
}
