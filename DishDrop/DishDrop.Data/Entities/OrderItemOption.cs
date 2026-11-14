using DishDrop.Data.Enums;

namespace DishDrop.Data.Entities;

public class OrderItemOption
{
    public int Id { get; set; }
    public int OrderItemId { get; set; }
    public int DishOptionId { get; set; }
    public string NameSnapshot { get; set; } = string.Empty;
    public decimal AdditionalPrice { get; set; }
    public OrderItem OrderItem { get; set; } = null!;
    public DishOption DishOption { get; set; } = null!;
}
