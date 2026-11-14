namespace DishDrop.Data;

/// <summary>Persisted lengths, SQL types and constraint expressions. Changes require a schema review.</summary>
public static class DataConstants
{
    public static class Identity
    {
        public const int UserIdMaxLength = 450;
        public const int LoginProviderMaxLength = 128;
        public const int ProviderKeyMaxLength = 128;
        public const int TokenNameMaxLength = 128;
    }

    public static class Money
    {
        public const int Precision = 18;
        public const int Scale = 2;
        public const string SqlType = "decimal(18,2)";
    }

    public static class Allergen
    {
        public const int CodeMaxLength = 30;
        public const int NameMaxLength = 150;
    }

    public static class ApplicationUser
    {
        public const int FirstNameMaxLength = 100;
        public const int LastNameMaxLength = 100;
        public const string NonemptyEmailFilter = "[NormalizedEmail] IS NOT NULL AND [NormalizedEmail] <> N''";
    }

    public static class AuditLog
    {
        public const int ActionMaxLength = 100;
        public const int EntityTypeMaxLength = 100;
        public const int EntityIdMaxLength = 450;
        public const int ReasonMaxLength = 1000;
        public const int CorrelationIdMaxLength = 100;
    }

    public static class CartItem
    {
        public const int SpecialInstructionsMaxLength = 1000;
        public const string QuantityCheckSql = "[Quantity] BETWEEN 1 AND 99";
    }

    public static class City
    {
        public const int NameMaxLength = 100;
        public const int NormalizedNameMaxLength = 100;
    }

    public static class CourierApprovalHistory
    {
        public const int ReasonMaxLength = 1000;
        public const string FromStatusCheckSql = "[FromStatus] IS NULL OR [FromStatus] BETWEEN 0 AND 2";
        public const string ToStatusCheckSql = "[ToStatus] BETWEEN 0 AND 2";
    }

    public static class CourierProfile
    {
        public const string ApprovalStatusCheckSql = "[ApprovalStatus] BETWEEN 0 AND 2";
        public const string VehicleTypeCheckSql = "[VehicleType] BETWEEN 0 AND 2";
    }

    public static class CuisineType
    {
        public const int NameMaxLength = 100;
        public const int NormalizedNameMaxLength = 100;
    }

    public static class CustomerAddress
    {
        public const int LabelMaxLength = 100;
        public const int StreetAddressMaxLength = 300;
        public const int BuildingMaxLength = 30;
        public const int EntranceMaxLength = 30;
        public const int FloorMaxLength = 30;
        public const int ApartmentMaxLength = 30;
        public const int DeliveryInstructionsMaxLength = 1000;
        public const string DefaultAddressFilter = "[IsDefault] = 1";
    }

    public static class DeliveryArea
    {
        public const int NameMaxLength = 100;
        public const int NormalizedNameMaxLength = 100;
    }

    public static class DeliveryAssignmentHistory
    {
        public const int ReleaseReasonMaxLength = 1000;
        public const string OpenAssignmentFilter = "[ReleasedAtUtc] IS NULL";
        public const string ReleaseCheckSql = "[ReleasedAtUtc] IS NULL OR [ReleasedAtUtc] >= [AssignedAtUtc]";
    }

    public static class Delivery
    {
        public const string ActiveCourierFilter = "[Status] IN (1, 2)";
        public const string CourierCheckSql = "([Status] = 0 AND [CourierId] IS NULL) OR ([Status] IN (1, 2, 3, 4) AND [CourierId] IS NOT NULL) OR [Status] = 5";
        public const string StatusCheckSql = "[Status] BETWEEN 0 AND 5";
    }

    public static class DeliveryStatusHistory
    {
        public const int ReasonMaxLength = 1000;
        public const string FromStatusCheckSql = "[FromStatus] IS NULL OR [FromStatus] BETWEEN 0 AND 5";
        public const string ToStatusCheckSql = "[ToStatus] BETWEEN 0 AND 5";
    }

    public static class Dish
    {
        public const int NameMaxLength = 150;
        public const int DescriptionMaxLength = 2000;
        public const int ImagePathMaxLength = 500;
    }

    public static class DishOption
    {
        public const int NameMaxLength = 150;
        public const string PriceCheckSql = "[AdditionalPrice] >= 0";
    }

    public static class DishOptionGroup
    {
        public const int NameMaxLength = 150;
        public const string SelectionsCheckSql = "[MinSelections] >= 0 AND [MinSelections] <= [MaxSelections] AND [MaxSelections] > 0";
    }

    public static class DishVariant
    {
        public const int NameMaxLength = 100;
        public const int NormalizedNameMaxLength = 100;
        public const int PortionQuantityPrecision = 18;
        public const int PortionQuantityScale = 3;
        public const string PortionQuantitySqlType = "decimal(18,3)";
        public const string PriceCheckSql = "[Price] > 0";
        public const string PortionCheckSql = "([PortionQuantity] IS NULL AND [PortionUnit] IS NULL) OR ([PortionQuantity] IS NOT NULL AND [PortionUnit] IS NOT NULL AND [PortionQuantity] > 0)";
        public const string PortionUnitCheckSql = "[PortionUnit] IS NULL OR [PortionUnit] BETWEEN 0 AND 2";
    }

    public static class MenuCategory
    {
        public const int NameMaxLength = 150;
        public const int NormalizedNameMaxLength = 150;
    }

    public static class Menu
    {
        public const int NameMaxLength = 150;
        public const int DescriptionMaxLength = 1000;
    }

    public static class Notification
    {
        public const int TitleMaxLength = 200;
        public const int MessageMaxLength = 2000;
        public const string UnreadFilter = "[ReadAtUtc] IS NULL";
        public const string TypeCheckSql = "[Type] BETWEEN 0 AND 4";
    }

    public static class Order
    {
        public const int OrderNumberMaxLength = 40;
        public const int CurrencyCodeMaxLength = 3;
        public const int CustomerNoteMaxLength = 1000;
        public const int CheckoutRequestHashMaxLength = 64;
        public const int RestaurantNameSnapshotMaxLength = 150;
        public const int PickupCitySnapshotMaxLength = 100;
        public const int PickupAddressSnapshotMaxLength = 300;
        public const int PickupPhoneSnapshotMaxLength = 30;
        public const string AmountsCheckSql = "[Subtotal] >= 0 AND [DeliveryFee] >= 0 AND [TotalAmount] = [Subtotal] + [DeliveryFee]";
        public const string CurrencyCheckSql = "LEN([CurrencyCode]) = 3 AND [CurrencyCode] COLLATE Latin1_General_100_BIN2 NOT LIKE '%[^A-Z]%'";
        public const string HashCheckSql = "LEN([CheckoutRequestHash]) = 64 AND [CheckoutRequestHash] COLLATE Latin1_General_100_BIN2 NOT LIKE '%[^0-9A-Fa-f]%'";
        public const string StatusCheckSql = "[Status] BETWEEN 0 AND 8";
    }

    public static class OrderDeliveryAddress
    {
        public const int RecipientNameMaxLength = 200;
        public const int PhoneNumberMaxLength = 30;
        public const int CityNameMaxLength = 100;
        public const int AreaNameMaxLength = 100;
        public const int StreetAddressMaxLength = 300;
        public const int BuildingMaxLength = 30;
        public const int EntranceMaxLength = 30;
        public const int FloorMaxLength = 30;
        public const int ApartmentMaxLength = 30;
        public const int DeliveryInstructionsMaxLength = 1000;
    }

    public static class OrderItem
    {
        public const int DishNameMaxLength = 150;
        public const int VariantNameMaxLength = 100;
        public const int SpecialInstructionsMaxLength = 1000;
        public const string AmountsCheckSql = "[BaseUnitPrice] > 0 AND [OptionsUnitPrice] >= 0 AND [LineTotal] = ([BaseUnitPrice] + [OptionsUnitPrice]) * [Quantity]";
        public const string QuantityCheckSql = "[Quantity] BETWEEN 1 AND 99";
    }

    public static class OrderItemOption
    {
        public const int NameSnapshotMaxLength = 150;
        public const string PriceCheckSql = "[AdditionalPrice] >= 0";
    }

    public static class OrderStatusHistory
    {
        public const int ReasonMaxLength = 1000;
        public const string FromStatusCheckSql = "[FromStatus] IS NULL OR [FromStatus] BETWEEN 0 AND 8";
        public const string ToStatusCheckSql = "[ToStatus] BETWEEN 0 AND 8";
    }

    public static class Payment
    {
        public const int CurrencyCodeMaxLength = 3;
        public const string AmountCheckSql = "[Amount] >= 0";
        public const string CurrencyCheckSql = "LEN([CurrencyCode]) = 3 AND [CurrencyCode] COLLATE Latin1_General_100_BIN2 NOT LIKE '%[^A-Z]%'";
        public const string MethodCheckSql = "[Method] BETWEEN 0 AND 0";
        public const string StatusCheckSql = "[Status] BETWEEN 0 AND 3";
    }

    public static class RestaurantApprovalHistory
    {
        public const int ReasonMaxLength = 1000;
        public const string FromStatusCheckSql = "[FromStatus] IS NULL OR [FromStatus] BETWEEN 0 AND 2";
        public const string ToStatusCheckSql = "[ToStatus] BETWEEN 0 AND 2";
    }

    public static class Restaurant
    {
        public const int NameMaxLength = 150;
        public const int SlugMaxLength = 180;
        public const int DescriptionMaxLength = 2000;
        public const int AddressMaxLength = 300;
        public const int PhoneNumberMaxLength = 30;
        public const int ImagePathMaxLength = 500;
        public const string MinimumOrderCheckSql = "[MinimumOrderAmount] >= 0";
        public const string PreparationCheckSql = "[EstimatedPreparationMinutes] > 0";
        public const string ApprovalStatusCheckSql = "[ApprovalStatus] BETWEEN 0 AND 2";
    }

    public static class RestaurantDeliveryArea
    {
        public const string FeeCheckSql = "[DeliveryFee] >= 0";
    }

    public static class RestaurantScheduleExceptionPeriod
    {
        public const string PeriodCheckSql = "([ClosesNextDay] = 0 AND [ClosesAt] > [OpensAt]) OR ([ClosesNextDay] = 1 AND [ClosesAt] < [OpensAt])";
    }

    public static class RestaurantStaff
    {
        public const string StaffRoleCheckSql = "[StaffRole] BETWEEN 0 AND 1";
    }

    public static class RestaurantWorkingPeriod
    {
        public const string PeriodCheckSql = "([ClosesNextDay] = 0 AND [ClosesAt] > [OpensAt]) OR ([ClosesNextDay] = 1 AND [ClosesAt] < [OpensAt])";
        public const string DayOfWeekCheckSql = "[DayOfWeek] BETWEEN 0 AND 6";
    }

    public static class Review
    {
        public const int CommentMaxLength = 2000;
        public const string RatingsCheckSql = "[RestaurantRating] BETWEEN 1 AND 5 AND ([DeliveryRating] IS NULL OR [DeliveryRating] BETWEEN 1 AND 5)";
        public const string ModerationStatusCheckSql = "[ModerationStatus] BETWEEN 0 AND 1";
    }

    public static class ReviewReply
    {
        public const int TextMaxLength = 2000;
        public const string ModerationStatusCheckSql = "[ModerationStatus] BETWEEN 0 AND 1";
    }

    public static class SupportTicket
    {
        public const int SubjectMaxLength = 200;
        public const string CategoryCheckSql = "[Category] BETWEEN 0 AND 3";
        public const string StatusCheckSql = "[Status] BETWEEN 0 AND 3";
    }

    public static class SupportTicketMessage
    {
        public const int TextMaxLength = 4000;
    }
}
