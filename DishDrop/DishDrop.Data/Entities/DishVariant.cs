using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using DishDrop.Data.Enums;
using static DishDrop.Data.DataConstants.DishVariant;

namespace DishDrop.Data.Entities
{
    [Table("DishVariant")]
    public class DishVariant
    {
        [Key]
        public int Id { get; set; }

        public int DishId { get; set; }

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(NormalizedNameMaxLength)]
        public string NormalizedName { get; set; } = null!;

        [Column(TypeName = DataConstants.Money.SqlType)]
        [Precision(DataConstants.Money.Precision, DataConstants.Money.Scale)]
        public decimal Price { get; set; }

        [Column(TypeName = PortionQuantitySqlType)]
        [Precision(PortionQuantityPrecision, PortionQuantityScale)]
        public decimal? PortionQuantity { get; set; }

        public PortionUnit? PortionUnit { get; set; }

        public bool IsActive { get; set; } = true;

        public bool IsAvailable { get; set; } = true;

        public int DisplayOrder { get; set; }

        [ForeignKey(nameof(DishId))]
        public Dish Dish { get; set; } = null!;

        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
