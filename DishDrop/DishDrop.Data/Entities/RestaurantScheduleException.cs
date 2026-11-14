using DishDrop.Data.Enums;

namespace DishDrop.Data.Entities;

public class RestaurantScheduleException
{
    public int Id { get; set; }
    public int RestaurantId { get; set; }
    public DateOnly Date { get; set; }
    public bool IsClosed { get; set; }
    public Restaurant Restaurant { get; set; } = null!;
}
