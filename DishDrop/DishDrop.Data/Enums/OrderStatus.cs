namespace DishDrop.Data.Enums;

// Persisted numeric values are stable; do not reorder or reuse them.
public enum OrderStatus
{
    New = 0,
    Accepted = 1,
    Preparing = 2,
    Ready = 3,
    Completed = 4,
    Rejected = 5,
    Cancelled = 6,
    RequiresResolution = 7,
    Failed = 8
}
