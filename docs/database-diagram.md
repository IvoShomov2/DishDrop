# ER диаграми

> **Диаграми на действителната текуща схема.** Опростяването от 16.11.2026 г. още не е приложено. Menu, варианти, staff и останалите показани таблици продължават да съществуват в кода/миграциите; присъствието им не ги прави задължителни за новата начална версия. Вижте [плана за промени](database-change-plan.md) и [новата спецификация](project-specification.md). Mermaid диаграмите не са подменени с бъдеща схема.

Групирани по области; външните за областта таблици са показани само като връзки.
ApplicationUser е AspNetUsers, останалите имена съвпадат с SQL таблиците.
Точните FK, nullable, checks и NO ACTION са в [database.md](database.md).

## Потребители, география и ресторанти

```mermaid
erDiagram
    ApplicationUser ||--o{ AspNetUserRoles : has
    AspNetRoles ||--o{ AspNetUserRoles : grants
    ApplicationUser {
        string Id PK
        string FirstName
        string LastName
        DateTime CreatedAtUtc
        bool IsActive
    }
    City {
        int Id PK
        string Name
        string NormalizedName
        bool IsActive
    }
    DeliveryArea {
        int Id PK
        int CityId FK
        string Name
        string NormalizedName
        bool IsActive
    }
    City ||--o{ DeliveryArea : City
    CustomerAddress {
        int Id PK
        string UserId FK
        int DeliveryAreaId FK
        string Label
        string StreetAddress
        string Building
        string Entrance
        string Floor
        string Apartment
        string DeliveryInstructions
        bool IsDefault
    }
    ApplicationUser ||--o{ CustomerAddress : User
    DeliveryArea ||--o{ CustomerAddress : DeliveryArea
    Restaurant {
        int Id PK
        int CityId FK
        string Name
        string Slug
        string Description
        string Address
        string PhoneNumber
        string ImagePath
        ApprovalStatus ApprovalStatus
        bool IsActive
        bool IsAcceptingOrders
        decimal MinimumOrderAmount
        int EstimatedPreparationMinutes
        DateTime CreatedAtUtc
        byte_array RowVersion
    }
    City ||--o{ Restaurant : City
    RestaurantStaff {
        int RestaurantId PK,FK
        string UserId PK,FK
        StaffRole StaffRole
        bool IsActive
        DateTime CreatedAtUtc
    }
    Restaurant ||--o{ RestaurantStaff : Restaurant
    ApplicationUser ||--o{ RestaurantStaff : User
    RestaurantWorkingPeriod {
        int Id PK
        int RestaurantId FK
        DayOfWeek DayOfWeek
        TimeOnly OpensAt
        TimeOnly ClosesAt
        bool ClosesNextDay
    }
    Restaurant ||--o{ RestaurantWorkingPeriod : Restaurant
    RestaurantScheduleException {
        int Id PK
        int RestaurantId FK
        DateOnly Date
        bool IsClosed
    }
    Restaurant ||--o{ RestaurantScheduleException : Restaurant
    RestaurantScheduleExceptionPeriod {
        int Id PK
        int RestaurantScheduleExceptionId FK
        TimeOnly OpensAt
        TimeOnly ClosesAt
        bool ClosesNextDay
    }
    RestaurantScheduleException ||--o{ RestaurantScheduleExceptionPeriod : RestaurantScheduleException
    RestaurantDeliveryArea {
        int RestaurantId PK,FK
        int DeliveryAreaId PK,FK
        decimal DeliveryFee
        bool IsActive
    }
    Restaurant ||--o{ RestaurantDeliveryArea : Restaurant
    DeliveryArea ||--o{ RestaurantDeliveryArea : DeliveryArea
    CuisineType {
        int Id PK
        string Name
        string NormalizedName
        bool IsActive
    }
    RestaurantCuisineType {
        int RestaurantId PK,FK
        int CuisineTypeId PK,FK
    }
    Restaurant ||--o{ RestaurantCuisineType : Restaurant
    CuisineType ||--o{ RestaurantCuisineType : CuisineType
    CourierProfile {
        string UserId PK,FK
        ApprovalStatus ApprovalStatus
        VehicleType VehicleType
        bool IsAvailable
        DateTime CreatedAtUtc
        DateTime ApprovedAtUtc
        byte_array RowVersion
    }
    ApplicationUser ||--o| CourierProfile : User
    CourierDeliveryArea {
        string CourierId PK,FK
        int DeliveryAreaId PK,FK
    }
    CourierProfile ||--o{ CourierDeliveryArea : Courier
    DeliveryArea ||--o{ CourierDeliveryArea : DeliveryArea
    RestaurantApprovalHistory {
        int Id PK
        int RestaurantId FK
        ApprovalStatus FromStatus
        ApprovalStatus ToStatus
        string ChangedByUserId FK
        DateTime ChangedAtUtc
        string Reason
    }
    Restaurant ||--o{ RestaurantApprovalHistory : Restaurant
    ApplicationUser ||--o{ RestaurantApprovalHistory : ChangedByUser
    CourierApprovalHistory {
        int Id PK
        string CourierId FK
        ApprovalStatus FromStatus
        ApprovalStatus ToStatus
        string ChangedByUserId FK
        DateTime ChangedAtUtc
        string Reason
    }
    CourierProfile ||--o{ CourierApprovalHistory : Courier
    ApplicationUser ||--o{ CourierApprovalHistory : ChangedByUser
```

## Меню и количка

```mermaid
erDiagram
    Menu {
        int Id PK
        int RestaurantId FK
        string Name
        string Description
        bool IsActive
        int DisplayOrder
    }
    Restaurant ||--o{ Menu : Restaurant
    MenuCategory {
        int Id PK
        int MenuId FK
        string Name
        string NormalizedName
        bool IsActive
        int DisplayOrder
    }
    Menu ||--o{ MenuCategory : Menu
    Dish {
        int Id PK
        int MenuCategoryId FK
        string Name
        string Description
        string ImagePath
        bool IsActive
        bool IsAvailable
        int DisplayOrder
        DateTime CreatedAtUtc
        byte_array RowVersion
    }
    MenuCategory ||--o{ Dish : MenuCategory
    DishVariant {
        int Id PK
        int DishId FK
        string Name
        string NormalizedName
        decimal Price
        decimal PortionQuantity
        PortionUnit PortionUnit
        bool IsActive
        bool IsAvailable
        int DisplayOrder
    }
    Dish ||--o{ DishVariant : Dish
    DishOptionGroup {
        int Id PK
        int DishId FK
        string Name
        int MinSelections
        int MaxSelections
        bool IsActive
        int DisplayOrder
    }
    Dish ||--o{ DishOptionGroup : Dish
    DishOption {
        int Id PK
        int DishOptionGroupId FK
        string Name
        decimal AdditionalPrice
        bool IsActive
        bool IsAvailable
        int DisplayOrder
    }
    DishOptionGroup ||--o{ DishOption : DishOptionGroup
    Allergen {
        int Id PK
        string Code
        string Name
    }
    DishAllergen {
        int DishId PK,FK
        int AllergenId PK,FK
    }
    Dish ||--o{ DishAllergen : Dish
    Allergen ||--o{ DishAllergen : Allergen
    DishOptionAllergen {
        int DishOptionId PK,FK
        int AllergenId PK,FK
    }
    DishOption ||--o{ DishOptionAllergen : DishOption
    Allergen ||--o{ DishOptionAllergen : Allergen
    Cart {
        int Id PK
        string UserId FK
        int RestaurantId FK
        DateTime UpdatedAtUtc
        byte_array RowVersion
    }
    ApplicationUser ||--o| Cart : User
    Restaurant |o--o{ Cart : Restaurant
    CartItem {
        int Id PK
        int CartId FK
        int DishVariantId FK
        int Quantity
        string SpecialInstructions
    }
    Cart ||--o{ CartItem : Cart
    DishVariant ||--o{ CartItem : DishVariant
    CartItemOption {
        int CartItemId PK,FK
        int DishOptionId PK,FK
    }
    CartItem ||--o{ CartItemOption : CartItem
    DishOption ||--o{ CartItemOption : DishOption
```

## Поръчки, плащания и доставки

```mermaid
erDiagram
    Order {
        int Id PK
        string OrderNumber
        string CustomerId FK
        int RestaurantId FK
        OrderStatus Status
        decimal Subtotal
        decimal DeliveryFee
        decimal TotalAmount
        string CurrencyCode
        string CustomerNote
        DateTime CreatedAtUtc
        DateTime EstimatedReadyAtUtc
        Guid CheckoutToken
        string CheckoutRequestHash
        string RestaurantNameSnapshot
        string PickupCitySnapshot
        string PickupAddressSnapshot
        string PickupPhoneSnapshot
        byte_array RowVersion
    }
    ApplicationUser ||--o{ Order : Customer
    Restaurant ||--o{ Order : Restaurant
    OrderDeliveryAddress {
        int OrderId PK,FK
        int DeliveryAreaId FK
        string RecipientName
        string PhoneNumber
        string CityName
        string AreaName
        string StreetAddress
        string Building
        string Entrance
        string Floor
        string Apartment
        string DeliveryInstructions
    }
    Order ||--o| OrderDeliveryAddress : Order
    DeliveryArea ||--o{ OrderDeliveryAddress : DeliveryArea
    OrderItem {
        int Id PK
        int OrderId FK
        int DishVariantId FK
        string DishName
        string VariantName
        decimal BaseUnitPrice
        decimal OptionsUnitPrice
        int Quantity
        decimal LineTotal
        string SpecialInstructions
    }
    Order ||--o{ OrderItem : Order
    DishVariant ||--o{ OrderItem : DishVariant
    OrderItemOption {
        int Id PK
        int OrderItemId FK
        int DishOptionId FK
        string NameSnapshot
        decimal AdditionalPrice
    }
    OrderItem ||--o{ OrderItemOption : OrderItem
    DishOption ||--o{ OrderItemOption : DishOption
    OrderStatusHistory {
        int Id PK
        int OrderId FK
        OrderStatus FromStatus
        OrderStatus ToStatus
        string ChangedByUserId FK
        DateTime ChangedAtUtc
        string Reason
    }
    Order ||--o{ OrderStatusHistory : Order
    ApplicationUser |o--o{ OrderStatusHistory : ChangedByUser
    Payment {
        int Id PK
        int OrderId FK
        PaymentMethod Method
        PaymentStatus Status
        decimal Amount
        string CurrencyCode
        DateTime CollectedAtUtc
        string CollectedByUserId FK
        byte_array RowVersion
    }
    Order ||--o| Payment : Order
    ApplicationUser |o--o{ Payment : CollectedByUser
    Delivery {
        int Id PK
        int OrderId FK
        string CourierId FK
        int DeliveryAreaId FK
        DeliveryStatus Status
        DateTime CreatedAtUtc
        DateTime AssignedAtUtc
        DateTime PickedUpAtUtc
        DateTime DeliveredAtUtc
        byte_array RowVersion
    }
    Order ||--o| Delivery : Order
    CourierProfile |o--o{ Delivery : Courier
    DeliveryArea ||--o{ Delivery : DeliveryArea
    DeliveryAssignmentHistory {
        int Id PK
        int DeliveryId FK
        string CourierId FK
        DateTime AssignedAtUtc
        DateTime ReleasedAtUtc
        string ReleaseReason
    }
    Delivery ||--o{ DeliveryAssignmentHistory : Delivery
    CourierProfile ||--o{ DeliveryAssignmentHistory : Courier
    DeliveryStatusHistory {
        int Id PK
        int DeliveryId FK
        DeliveryStatus FromStatus
        DeliveryStatus ToStatus
        string ChangedByUserId FK
        DateTime ChangedAtUtc
        string Reason
    }
    Delivery ||--o{ DeliveryStatusHistory : Delivery
    ApplicationUser |o--o{ DeliveryStatusHistory : ChangedByUser
```

## Отзиви, поддръжка и известия

```mermaid
erDiagram
    Review {
        int Id PK
        int OrderId FK
        int RestaurantRating
        int DeliveryRating
        string Comment
        DateTime CreatedAtUtc
        DateTime UpdatedAtUtc
        ModerationStatus ModerationStatus
        byte_array RowVersion
    }
    Order ||--o| Review : Order
    ReviewReply {
        int ReviewId PK,FK
        string AuthorUserId FK
        string Text
        DateTime CreatedAtUtc
        DateTime UpdatedAtUtc
        ModerationStatus ModerationStatus
    }
    Review ||--o| ReviewReply : Review
    ApplicationUser ||--o{ ReviewReply : AuthorUser
    SupportTicket {
        int Id PK
        string CreatedByUserId FK
        int OrderId FK
        SupportCategory Category
        string Subject
        SupportStatus Status
        string AssignedAdminId FK
        DateTime CreatedAtUtc
        DateTime UpdatedAtUtc
        byte_array RowVersion
    }
    ApplicationUser ||--o{ SupportTicket : CreatedByUser
    Order |o--o{ SupportTicket : Order
    ApplicationUser |o--o{ SupportTicket : AssignedAdmin
    SupportTicketMessage {
        int Id PK
        int SupportTicketId FK
        string AuthorUserId FK
        string Text
        DateTime CreatedAtUtc
    }
    SupportTicket ||--o{ SupportTicketMessage : SupportTicket
    ApplicationUser ||--o{ SupportTicketMessage : AuthorUser
    Notification {
        int Id PK
        string RecipientUserId FK
        NotificationType Type
        string Title
        string Message
        int OrderId FK
        int SupportTicketId FK
        DateTime CreatedAtUtc
        DateTime ReadAtUtc
    }
    ApplicationUser ||--o{ Notification : RecipientUser
    Order |o--o{ Notification : Order
    SupportTicket |o--o{ Notification : SupportTicket
    AuditLog {
        int Id PK
        string ActorUserId FK
        string Action
        string EntityType
        string EntityId
        DateTime CreatedAtUtc
        string Reason
        string CorrelationId
    }
    ApplicationUser |o--o{ AuditLog : ActorUser
```
