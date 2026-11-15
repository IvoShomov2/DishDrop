using System.ComponentModel.DataAnnotations.Schema;

namespace DishDrop.Data.Entities
{
    [Table("CartItemOption")]
    public class CartItemOption
    {
        public int CartItemId { get; set; }

        public int DishOptionId { get; set; }

        [ForeignKey(nameof(CartItemId))]
        public CartItem CartItem { get; set; } = null!;

        [ForeignKey(nameof(DishOptionId))]
        public DishOption DishOption { get; set; } = null!;
    }
}
