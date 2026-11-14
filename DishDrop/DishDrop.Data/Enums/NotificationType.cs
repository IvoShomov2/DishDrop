namespace DishDrop.Data.Enums;

// Persisted numeric values are stable; do not reorder or reuse them.
public enum NotificationType
{
    OrderUpdated = 0,
    DeliveryUpdated = 1,
    SupportReply = 2,
    RestaurantApprovalUpdated = 3,
    CourierApprovalUpdated = 4
}
