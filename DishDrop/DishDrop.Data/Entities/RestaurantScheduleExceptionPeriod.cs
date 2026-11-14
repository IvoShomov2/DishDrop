using DishDrop.Data.Enums;

namespace DishDrop.Data.Entities;

public class RestaurantScheduleExceptionPeriod
{
    public int Id { get; set; }
    public int RestaurantScheduleExceptionId { get; set; }
    public TimeOnly OpensAt { get; set; }
    public TimeOnly ClosesAt { get; set; }
    public bool ClosesNextDay { get; set; }
    public RestaurantScheduleException RestaurantScheduleException { get; set; } = null!;
}
