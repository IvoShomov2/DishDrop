using DishDrop.Data.Enums;

namespace DishDrop.Data.Entities;

public class OrderItem
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public int DishVariantId { get; set; }
    public string DishName { get; set; } = string.Empty;
    public string VariantName { get; set; } = string.Empty;
    public decimal BaseUnitPrice { get; set; }
    public decimal OptionsUnitPrice { get; set; }
    public int Quantity { get; set; }
    public decimal LineTotal { get; set; }
    public string? SpecialInstructions { get; set; }
    public Order Order { get; set; } = null!;
    public DishVariant DishVariant { get; set; } = null!;
}
