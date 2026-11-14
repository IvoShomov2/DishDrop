using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static DishDrop.Data.DataConstants.CartItem;

namespace DishDrop.Data.Entities
{
    [Table("CartItem")]
    public class CartItem
    {
        [Key]
        public int Id { get; set; }

        public int CartId { get; set; }

        public int DishVariantId { get; set; }

        public int Quantity { get; set; }

        [MaxLength(SpecialInstructionsMaxLength)]
        public string? SpecialInstructions { get; set; }

        [ForeignKey(nameof(CartId))]
        public Cart Cart { get; set; } = null!;

        [ForeignKey(nameof(DishVariantId))]
        public DishVariant DishVariant { get; set; } = null!;

        public ICollection<CartItemOption> Options { get; set; } = new List<CartItemOption>();
    }
}
