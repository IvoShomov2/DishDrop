using DishDrop.Data.Enums;

namespace DishDrop.Data.Entities;

public class CartItemOption
{
    public int CartItemId { get; set; }
    public int DishOptionId { get; set; }
    public CartItem CartItem { get; set; } = null!;
    public DishOption DishOption { get; set; } = null!;
}
