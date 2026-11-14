using DishDrop.Data.Enums;

namespace DishDrop.Data.Entities;

public class RestaurantCuisineType
{
    public int RestaurantId { get; set; }
    public int CuisineTypeId { get; set; }
    public Restaurant Restaurant { get; set; } = null!;
    public CuisineType CuisineType { get; set; } = null!;
}
