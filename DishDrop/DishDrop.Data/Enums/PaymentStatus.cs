namespace DishDrop.Data.Enums;

// Persisted numeric values are stable; do not reorder or reuse them.
public enum PaymentStatus
{
    Pending = 0,
    Paid = 1,
    Cancelled = 2,
    RequiresReview = 3
}
