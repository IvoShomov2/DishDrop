using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using static DishDrop.Data.DataConstants.OrderItem;

namespace DishDrop.Data.Entities
{
    [Table("OrderItem")]
    public class OrderItem
    {
        [Key]
        public int Id { get; set; }

        public int OrderId { get; set; }

        public int DishVariantId { get; set; }

        [Required]
        [MaxLength(DishNameMaxLength)]
        public string DishName { get; set; } = null!;

        [Required]
        [MaxLength(VariantNameMaxLength)]
        public string VariantName { get; set; } = null!;

        [Column(TypeName = DataConstants.Money.SqlType)]
        [Precision(DataConstants.Money.Precision, DataConstants.Money.Scale)]
        public decimal BaseUnitPrice { get; set; }

        [Column(TypeName = DataConstants.Money.SqlType)]
        [Precision(DataConstants.Money.Precision, DataConstants.Money.Scale)]
        public decimal OptionsUnitPrice { get; set; }

        public int Quantity { get; set; }

        [Column(TypeName = DataConstants.Money.SqlType)]
        [Precision(DataConstants.Money.Precision, DataConstants.Money.Scale)]
        public decimal LineTotal { get; set; }

        [MaxLength(SpecialInstructionsMaxLength)]
        public string? SpecialInstructions { get; set; }

        [ForeignKey(nameof(OrderId))]
        public Order Order { get; set; } = null!;

        [ForeignKey(nameof(DishVariantId))]
        public DishVariant DishVariant { get; set; } = null!;

        public ICollection<OrderItemOption> Options { get; set; } = new List<OrderItemOption>();
    }
}
