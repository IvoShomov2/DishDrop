using DishDrop.Data.Enums;

namespace DishDrop.Data.Entities;

public class RestaurantWorkingPeriod
{
    public int Id { get; set; }
    public int RestaurantId { get; set; }
    public DayOfWeek DayOfWeek { get; set; }
    public TimeOnly OpensAt { get; set; }
    public TimeOnly ClosesAt { get; set; }
    public bool ClosesNextDay { get; set; }
    public Restaurant Restaurant { get; set; } = null!;
}
