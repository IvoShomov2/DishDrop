namespace DishDrop.Data;

public static class RoleNames
{
    public const string Customer = "Customer";
    public const string Restaurant = "Restaurant";
    public const string Courier = "Courier";
    public const string Admin = "Admin";
    public static IReadOnlyList<string> All { get; } =
        Array.AsReadOnly(new[] { Customer, Restaurant, Courier, Admin });
}
