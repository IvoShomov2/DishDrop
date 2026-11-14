using DishDrop.Data.Enums;

namespace DishDrop.Data.Entities;

public class CartItem
{
    public int Id { get; set; }
    public int CartId { get; set; }
    public int DishVariantId { get; set; }
    public int Quantity { get; set; }
    public string? SpecialInstructions { get; set; }
    public Cart Cart { get; set; } = null!;
    public DishVariant DishVariant { get; set; } = null!;
}
