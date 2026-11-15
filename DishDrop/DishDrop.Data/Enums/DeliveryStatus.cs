namespace DishDrop.Data.Enums;

// Persisted numeric values are stable; do not reorder or reuse them.
public enum DeliveryStatus
{
    AwaitingCourier = 0,
    Assigned = 1,
    PickedUp = 2,
    Delivered = 3,
    Failed = 4,
    Cancelled = 5
}
