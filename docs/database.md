# База данни на DishDrop

## Статус и общи решения

Моделът е реализиран в Data с атрибути в entities, DataConstants за дължини,
SQL типове и CHECK/filter изрази, и допълнителен Fluent API директно в
ApplicationDbContext.OnModelCreating, групиран по области без отделни Configure методи.
Required/MaxLength, Table/Column, Key/ForeignKey и Timestamp са в entities.
Precision запазва оригиналните precision/scale facets заедно с Column SQL типа.
Nullable свойствата не получават Required. SQL CHECK ограниченията остават
изрично регистрирани в DbContext, без заместване с Range.
Съставните PK (включително NONCLUSTERED), relationships с NO ACTION, индекси,
defaults и стандартните Identity key lengths също се задават там след base.OnModelCreating.
Има 42 нови бизнес таблици и разширена AspNetUsers; стандартните
AspNetRoles, AspNetUserRoles, AspNetUserClaims, AspNetUserLogins, AspNetUserTokens и
AspNetRoleClaims са запазени. ApplicationUser наследява IdentityUser със string ключ.
Другите ключове са int IDENTITY, освен съставните PK и PK/FK записите.
Съставните PK на RestaurantStaff и CourierDeliveryArea са NONCLUSTERED:
nvarchar(450) + int може да надвиши 900-byte clustered key лимита. Така
оригиналната максимална дължина на Identity ключа остава поддържана;
това е проверено с 450-character user ID върху SQL Server.
Всяка FK има явна navigation property; няма неочаквани shadow FK.

Парите са decimal(18,2); PortionQuantity е decimal(18,3). Начална валута EUR,
кодът е точно три главни ASCII букви. CheckoutToken е uniqueidentifier,
CheckoutRequestHash е 64 hex знака (бъдещ SHA-256 hash на канонизирана заявка).
datetime2 полетата с Utc са UTC по договор; SQL типът не пази timezone.
Бъдещите услуги трябва да подават UTC. Работното време е time и date,
локално за Europe/Sofia; DayOfWeek е .NET Sunday=0 до Saturday=6.
Няма GPS/geocoding и автоматична проверка на улица спрямо район.

Всеки enum се пази като int с изрично зададени, непренареждаеми числови стойности
и CHECK за домейна. Това проверява допустимата стойност, а не преходите.
DeliveryStatus: AwaitingCourier=0, Assigned=1, PickedUp=2, Delivered=3, Failed=4,
Cancelled=5. Индексът върху CourierId използва SQL Server predicate
`[Status] IN (1, 2)`; той е приложен и доказан върху LocalDB SQL Server.

## Изтриване и конкуренция

Всички FK на бизнес модела, включително към ApplicationUser, са NO ACTION.
Физическо изтриване на използван каталог/акаунт/поръчка се отказва от SQL Server.
Така няма multiple cascade paths и не се заличават финансови/доставни/админ истории.
Оригиналните вътрешни Identity cascade връзки са непроменени.
Деактивирайте използваните каталогови записи. Изтриване на цял агрегат, ако бъде
необходимо за празна количка, изисква явна операция за зависимостите.

RowVersion е SQL Server rowversion (8-byte concurrency token) при Restaurant,
CourierProfile, Dish, Cart, Order, Payment, Delivery, Review и SupportTicket.
EF отказва stale update с DbUpdateConcurrencyException. Това не замества
транзакционната координация между агрегати. Няма механични IsDeleted/timestamps
или rowversion по свързващите таблици. Съществуващият Identity ConcurrencyStamp остава.

## Identity и миграции

RequireUniqueEmail=true и filtered unique EmailIndex за непразен NormalizedEmail.
NULL/празни legacy стойности остават разрешени. Имената са NOT NULL с max 100.
За старите потребители FirstName/LastName получават празен низ (неизвестни),
IsActive=true, CreatedAtUtc=момента на upgrade-а, а не измислена историческа дата.
Register изисква имена; Manage позволява попълването им. Новите имена не се
извличат от email. RequireConfirmedAccount=true е запазено. Ролите са стандартни
Identity роли, много към много; RoleNames съдържа четирите централни имена.
Регистрацията не дава привилегировани роли; бъдещото provisioning/authorization
трябва да използва отделни операции. IsActive е моделно поле; административният
процес за блокиране/отнемане на сесии още не е реализиран.

Запазена е 00000000000000_CreateIdentitySchema. Новата миграция е
20261114175432_AddInitialBusinessSchema. Up няма DropTable/DropColumn, а само
заменя EmailIndex с уникален индекс, добавя четири user колони и бизнес таблиците.
Преди промени се проверяват duplicate nonempty NormalizedEmail и се хвърля SQL
грешка 51000 с диагностика; няма автоматично сливане/изтриване.
Down е разрушителен rollback на новата схема и не е начин за поправка на данни.

Предаваният идемпотентен скрипт е [sql/initial-business-schema.sql](sql/initial-business-schema.sql).
Съдържа двете миграции и проверки по __EFMigrationsHistory, без credentials,
пароли, demo акаунти или connection string. Двукратно изпълнение е тествано.
Изпълнява се в предварително избрана база; няма USE/CREATE DATABASE.
Startup няма EnsureCreated или автоматични migrations.

## Каталог на таблиците

По-долу ? означава nullable. Останалите полета са NOT NULL; int PK е IDENTITY,
освен PK/FK. sN е nvarchar(N), byte[] RowVersion е rowversion.
Всички FK по-долу са NO ACTION. EF създава и стандартни FK индекси, ако не са
покрити от PK/друг индекс. Посочените допълнителни индекси са явните в конфигурациите.
При AspNetUsers са изброени само новите полета; стандартните Identity полета
(Id, UserName, Email, hashes, stamps, lockout и др.) остават без загуба.

### ApplicationUser → AspNetUsers

PK: Id.

| Поле | Тип | Връзка |
| --- | --- | --- |
| FirstName | s100 |  |
| LastName | s100 |  |
| CreatedAtUtc | DateTime |  |
| IsActive | bool |  |

UNIQUE EmailIndex WHERE NormalizedEmail IS NOT NULL AND NormalizedEmail <> N''.

### City

PK: Id.

| Поле | Тип | Връзка |
| --- | --- | --- |
| Id | int |  |
| Name | s100 |  |
| NormalizedName | s100 |  |
| IsActive | bool |  |

Индекси: UNIQUE NormalizedName.

### DeliveryArea

PK: Id.

| Поле | Тип | Връзка |
| --- | --- | --- |
| Id | int |  |
| CityId | int | City (City) |
| Name | s100 |  |
| NormalizedName | s100 |  |
| IsActive | bool |  |

Индекси: UNIQUE CityId,NormalizedName.

### CustomerAddress

PK: Id.

| Поле | Тип | Връзка |
| --- | --- | --- |
| Id | int |  |
| UserId | s450 | ApplicationUser (User) |
| DeliveryAreaId | int | DeliveryArea (DeliveryArea) |
| Label | s100 |  |
| StreetAddress | s300 |  |
| Building | s30? |  |
| Entrance | s30? |  |
| Floor | s30? |  |
| Apartment | s30? |  |
| DeliveryInstructions | s1000? |  |
| IsDefault | bool |  |

Индекси: UNIQUE UserId WHERE [IsDefault] = 1.

### Restaurant

PK: Id.

| Поле | Тип | Връзка |
| --- | --- | --- |
| Id | int |  |
| CityId | int | City (City) |
| Name | s150 |  |
| Slug | s180 |  |
| Description | s2000 |  |
| Address | s300 |  |
| PhoneNumber | s30 |  |
| ImagePath | s500? |  |
| ApprovalStatus | ApprovalStatus |  |
| IsActive | bool |  |
| IsAcceptingOrders | bool |  |
| MinimumOrderAmount | decimal |  |
| EstimatedPreparationMinutes | int |  |
| CreatedAtUtc | DateTime |  |
| RowVersion | byte[] |  |

Индекси: UNIQUE Slug.

CHECK правила:

- MinimumOrder: `[MinimumOrderAmount] >= 0`
- Preparation: `[EstimatedPreparationMinutes] > 0`
- ApprovalStatus: `[ApprovalStatus] BETWEEN 0 AND 2`

### RestaurantStaff

PK: RestaurantId, UserId.

| Поле | Тип | Връзка |
| --- | --- | --- |
| RestaurantId | int | Restaurant (Restaurant) |
| UserId | s450 | ApplicationUser (User) |
| StaffRole | StaffRole |  |
| IsActive | bool |  |
| CreatedAtUtc | DateTime |  |

CHECK правила:

- StaffRole: `[StaffRole] BETWEEN 0 AND 1`

### RestaurantWorkingPeriod

PK: Id.

| Поле | Тип | Връзка |
| --- | --- | --- |
| Id | int |  |
| RestaurantId | int | Restaurant (Restaurant) |
| DayOfWeek | DayOfWeek |  |
| OpensAt | TimeOnly |  |
| ClosesAt | TimeOnly |  |
| ClosesNextDay | bool |  |

Индекси: RestaurantId,DayOfWeek.

CHECK правила:

- Period: `([ClosesNextDay] = 0 AND [ClosesAt] > [OpensAt]) OR ([ClosesNextDay] = 1 AND [ClosesAt] < [OpensAt])`
- DayOfWeek: `[DayOfWeek] BETWEEN 0 AND 6`

### RestaurantScheduleException

PK: Id.

| Поле | Тип | Връзка |
| --- | --- | --- |
| Id | int |  |
| RestaurantId | int | Restaurant (Restaurant) |
| Date | DateOnly |  |
| IsClosed | bool |  |

Индекси: UNIQUE RestaurantId,Date.

### RestaurantScheduleExceptionPeriod

PK: Id.

| Поле | Тип | Връзка |
| --- | --- | --- |
| Id | int |  |
| RestaurantScheduleExceptionId | int | RestaurantScheduleException (RestaurantScheduleException) |
| OpensAt | TimeOnly |  |
| ClosesAt | TimeOnly |  |
| ClosesNextDay | bool |  |

CHECK правила:

- Period: `([ClosesNextDay] = 0 AND [ClosesAt] > [OpensAt]) OR ([ClosesNextDay] = 1 AND [ClosesAt] < [OpensAt])`

### RestaurantDeliveryArea

PK: RestaurantId, DeliveryAreaId.

| Поле | Тип | Връзка |
| --- | --- | --- |
| RestaurantId | int | Restaurant (Restaurant) |
| DeliveryAreaId | int | DeliveryArea (DeliveryArea) |
| DeliveryFee | decimal |  |
| IsActive | bool |  |

CHECK правила:

- Fee: `[DeliveryFee] >= 0`

### CuisineType

PK: Id.

| Поле | Тип | Връзка |
| --- | --- | --- |
| Id | int |  |
| Name | s100 |  |
| NormalizedName | s100 |  |
| IsActive | bool |  |

Индекси: UNIQUE NormalizedName.

### RestaurantCuisineType

PK: RestaurantId, CuisineTypeId.

| Поле | Тип | Връзка |
| --- | --- | --- |
| RestaurantId | int | Restaurant (Restaurant) |
| CuisineTypeId | int | CuisineType (CuisineType) |

### CourierProfile

PK: UserId.

| Поле | Тип | Връзка |
| --- | --- | --- |
| UserId | s450 | ApplicationUser (User) |
| ApprovalStatus | ApprovalStatus |  |
| VehicleType | VehicleType |  |
| IsAvailable | bool |  |
| CreatedAtUtc | DateTime |  |
| ApprovedAtUtc | DateTime? |  |
| RowVersion | byte[] |  |

CHECK правила:

- ApprovalStatus: `[ApprovalStatus] BETWEEN 0 AND 2`
- VehicleType: `[VehicleType] BETWEEN 0 AND 2`

### CourierDeliveryArea

PK: CourierId, DeliveryAreaId.

| Поле | Тип | Връзка |
| --- | --- | --- |
| CourierId | s450 | CourierProfile (Courier) |
| DeliveryAreaId | int | DeliveryArea (DeliveryArea) |

### RestaurantApprovalHistory

PK: Id.

| Поле | Тип | Връзка |
| --- | --- | --- |
| Id | int |  |
| RestaurantId | int | Restaurant (Restaurant) |
| FromStatus | ApprovalStatus? |  |
| ToStatus | ApprovalStatus |  |
| ChangedByUserId | s450 | ApplicationUser (ChangedByUser) |
| ChangedAtUtc | DateTime |  |
| Reason | s1000 |  |

Индекси: RestaurantId,ChangedAtUtc.

CHECK правила:

- FromStatus: `[FromStatus] IS NULL OR [FromStatus] BETWEEN 0 AND 2`
- ToStatus: `[ToStatus] BETWEEN 0 AND 2`

### CourierApprovalHistory

PK: Id.

| Поле | Тип | Връзка |
| --- | --- | --- |
| Id | int |  |
| CourierId | s450 | CourierProfile (Courier) |
| FromStatus | ApprovalStatus? |  |
| ToStatus | ApprovalStatus |  |
| ChangedByUserId | s450 | ApplicationUser (ChangedByUser) |
| ChangedAtUtc | DateTime |  |
| Reason | s1000 |  |

Индекси: CourierId,ChangedAtUtc.

CHECK правила:

- FromStatus: `[FromStatus] IS NULL OR [FromStatus] BETWEEN 0 AND 2`
- ToStatus: `[ToStatus] BETWEEN 0 AND 2`

### Menu

PK: Id.

| Поле | Тип | Връзка |
| --- | --- | --- |
| Id | int |  |
| RestaurantId | int | Restaurant (Restaurant) |
| Name | s150 |  |
| Description | s1000? |  |
| IsActive | bool |  |
| DisplayOrder | int |  |

Индекси: RestaurantId,DisplayOrder.

### MenuCategory

PK: Id.

| Поле | Тип | Връзка |
| --- | --- | --- |
| Id | int |  |
| MenuId | int | Menu (Menu) |
| Name | s150 |  |
| NormalizedName | s150 |  |
| IsActive | bool |  |
| DisplayOrder | int |  |

Индекси: UNIQUE MenuId,NormalizedName.

### Dish

PK: Id.

| Поле | Тип | Връзка |
| --- | --- | --- |
| Id | int |  |
| MenuCategoryId | int | MenuCategory (MenuCategory) |
| Name | s150 |  |
| Description | s2000 |  |
| ImagePath | s500? |  |
| IsActive | bool |  |
| IsAvailable | bool |  |
| DisplayOrder | int |  |
| CreatedAtUtc | DateTime |  |
| RowVersion | byte[] |  |

Индекси: MenuCategoryId,DisplayOrder.

### DishVariant

PK: Id.

| Поле | Тип | Връзка |
| --- | --- | --- |
| Id | int |  |
| DishId | int | Dish (Dish) |
| Name | s100 |  |
| NormalizedName | s100 |  |
| Price | decimal |  |
| PortionQuantity | decimal? |  |
| PortionUnit | PortionUnit? |  |
| IsActive | bool |  |
| IsAvailable | bool |  |
| DisplayOrder | int |  |

Индекси: UNIQUE DishId,NormalizedName.

CHECK правила:

- Price: `[Price] > 0`
- Portion: `([PortionQuantity] IS NULL AND [PortionUnit] IS NULL) OR ([PortionQuantity] IS NOT NULL AND [PortionUnit] IS NOT NULL AND [PortionQuantity] > 0)`
- PortionUnit: `[PortionUnit] IS NULL OR [PortionUnit] BETWEEN 0 AND 2`

### DishOptionGroup

PK: Id.

| Поле | Тип | Връзка |
| --- | --- | --- |
| Id | int |  |
| DishId | int | Dish (Dish) |
| Name | s150 |  |
| MinSelections | int |  |
| MaxSelections | int |  |
| IsActive | bool |  |
| DisplayOrder | int |  |

CHECK правила:

- Selections: `[MinSelections] >= 0 AND [MinSelections] <= [MaxSelections] AND [MaxSelections] > 0`

### DishOption

PK: Id.

| Поле | Тип | Връзка |
| --- | --- | --- |
| Id | int |  |
| DishOptionGroupId | int | DishOptionGroup (DishOptionGroup) |
| Name | s150 |  |
| AdditionalPrice | decimal |  |
| IsActive | bool |  |
| IsAvailable | bool |  |
| DisplayOrder | int |  |

CHECK правила:

- Price: `[AdditionalPrice] >= 0`

### Allergen

PK: Id.

| Поле | Тип | Връзка |
| --- | --- | --- |
| Id | int |  |
| Code | s30 |  |
| Name | s150 |  |

Индекси: UNIQUE Code.

### DishAllergen

PK: DishId, AllergenId.

| Поле | Тип | Връзка |
| --- | --- | --- |
| DishId | int | Dish (Dish) |
| AllergenId | int | Allergen (Allergen) |

### DishOptionAllergen

PK: DishOptionId, AllergenId.

| Поле | Тип | Връзка |
| --- | --- | --- |
| DishOptionId | int | DishOption (DishOption) |
| AllergenId | int | Allergen (Allergen) |

### Cart

PK: Id.

| Поле | Тип | Връзка |
| --- | --- | --- |
| Id | int |  |
| UserId | s450 | ApplicationUser (User) |
| RestaurantId | int? | Restaurant (Restaurant) |
| UpdatedAtUtc | DateTime |  |
| RowVersion | byte[] |  |

Индекси: UNIQUE UserId.

### CartItem

PK: Id.

| Поле | Тип | Връзка |
| --- | --- | --- |
| Id | int |  |
| CartId | int | Cart (Cart) |
| DishVariantId | int | DishVariant (DishVariant) |
| Quantity | int |  |
| SpecialInstructions | s1000? |  |

CHECK правила:

- Quantity: `[Quantity] BETWEEN 1 AND 99`

### CartItemOption

PK: CartItemId, DishOptionId.

| Поле | Тип | Връзка |
| --- | --- | --- |
| CartItemId | int | CartItem (CartItem) |
| DishOptionId | int | DishOption (DishOption) |

### Order

PK: Id.

| Поле | Тип | Връзка |
| --- | --- | --- |
| Id | int |  |
| OrderNumber | s40 |  |
| CustomerId | s450 | ApplicationUser (Customer) |
| RestaurantId | int | Restaurant (Restaurant) |
| Status | OrderStatus |  |
| Subtotal | decimal |  |
| DeliveryFee | decimal |  |
| TotalAmount | decimal |  |
| CurrencyCode | s3 |  |
| CustomerNote | s1000? |  |
| CreatedAtUtc | DateTime |  |
| EstimatedReadyAtUtc | DateTime? |  |
| CheckoutToken | Guid |  |
| CheckoutRequestHash | s64 |  |
| RestaurantNameSnapshot | s150 |  |
| PickupCitySnapshot | s100 |  |
| PickupAddressSnapshot | s300 |  |
| PickupPhoneSnapshot | s30 |  |
| RowVersion | byte[] |  |

Индекси: UNIQUE OrderNumber; UNIQUE CheckoutToken; CustomerId,CreatedAtUtc; RestaurantId,Status,CreatedAtUtc.

CHECK правила:

- Amounts: `[Subtotal] >= 0 AND [DeliveryFee] >= 0 AND [TotalAmount] = [Subtotal] + [DeliveryFee]`
- Currency: `LEN([CurrencyCode]) = 3 AND [CurrencyCode] COLLATE Latin1_General_100_BIN2 NOT LIKE '%[^A-Z]%'`
- Hash: `LEN([CheckoutRequestHash]) = 64 AND [CheckoutRequestHash] COLLATE Latin1_General_100_BIN2 NOT LIKE '%[^0-9A-Fa-f]%'`
- Status: `[Status] BETWEEN 0 AND 8`

### OrderDeliveryAddress

PK: OrderId.

| Поле | Тип | Връзка |
| --- | --- | --- |
| OrderId | int | Order (Order) |
| DeliveryAreaId | int | DeliveryArea (DeliveryArea) |
| RecipientName | s200 |  |
| PhoneNumber | s30 |  |
| CityName | s100 |  |
| AreaName | s100 |  |
| StreetAddress | s300 |  |
| Building | s30? |  |
| Entrance | s30? |  |
| Floor | s30? |  |
| Apartment | s30? |  |
| DeliveryInstructions | s1000? |  |

### OrderItem

PK: Id.

| Поле | Тип | Връзка |
| --- | --- | --- |
| Id | int |  |
| OrderId | int | Order (Order) |
| DishVariantId | int | DishVariant (DishVariant) |
| DishName | s150 |  |
| VariantName | s100 |  |
| BaseUnitPrice | decimal |  |
| OptionsUnitPrice | decimal |  |
| Quantity | int |  |
| LineTotal | decimal |  |
| SpecialInstructions | s1000? |  |

CHECK правила:

- Amounts: `[BaseUnitPrice] > 0 AND [OptionsUnitPrice] >= 0 AND [LineTotal] = ([BaseUnitPrice] + [OptionsUnitPrice]) * [Quantity]`
- Quantity: `[Quantity] BETWEEN 1 AND 99`

### OrderItemOption

PK: Id.

| Поле | Тип | Връзка |
| --- | --- | --- |
| Id | int |  |
| OrderItemId | int | OrderItem (OrderItem) |
| DishOptionId | int | DishOption (DishOption) |
| NameSnapshot | s150 |  |
| AdditionalPrice | decimal |  |

Индекси: UNIQUE OrderItemId,DishOptionId.

CHECK правила:

- Price: `[AdditionalPrice] >= 0`

### OrderStatusHistory

PK: Id.

| Поле | Тип | Връзка |
| --- | --- | --- |
| Id | int |  |
| OrderId | int | Order (Order) |
| FromStatus | OrderStatus? |  |
| ToStatus | OrderStatus |  |
| ChangedByUserId | s450? | ApplicationUser (ChangedByUser) |
| ChangedAtUtc | DateTime |  |
| Reason | s1000 |  |

Индекси: OrderId,ChangedAtUtc.

CHECK правила:

- FromStatus: `[FromStatus] IS NULL OR [FromStatus] BETWEEN 0 AND 8`
- ToStatus: `[ToStatus] BETWEEN 0 AND 8`

### Payment

PK: Id.

| Поле | Тип | Връзка |
| --- | --- | --- |
| Id | int |  |
| OrderId | int | Order (Order) |
| Method | PaymentMethod |  |
| Status | PaymentStatus |  |
| Amount | decimal |  |
| CurrencyCode | s3 |  |
| CollectedAtUtc | DateTime? |  |
| CollectedByUserId | s450? | ApplicationUser (CollectedByUser) |
| RowVersion | byte[] |  |

Индекси: UNIQUE OrderId.

CHECK правила:

- Amount: `[Amount] >= 0`
- Currency: `LEN([CurrencyCode]) = 3 AND [CurrencyCode] COLLATE Latin1_General_100_BIN2 NOT LIKE '%[^A-Z]%'`
- Method: `[Method] BETWEEN 0 AND 0`
- Status: `[Status] BETWEEN 0 AND 3`

### Delivery

PK: Id.

| Поле | Тип | Връзка |
| --- | --- | --- |
| Id | int |  |
| OrderId | int | Order (Order) |
| CourierId | s450? | CourierProfile (Courier) |
| DeliveryAreaId | int | DeliveryArea (DeliveryArea) |
| Status | DeliveryStatus |  |
| CreatedAtUtc | DateTime |  |
| AssignedAtUtc | DateTime? |  |
| PickedUpAtUtc | DateTime? |  |
| DeliveredAtUtc | DateTime? |  |
| RowVersion | byte[] |  |

Индекси: UNIQUE OrderId; UNIQUE CourierId WHERE [Status] IN (1, 2).

CHECK правила:

- Courier: `([Status] = 0 AND [CourierId] IS NULL) OR ([Status] IN (1, 2, 3, 4) AND [CourierId] IS NOT NULL) OR [Status] = 5`
- Status: `[Status] BETWEEN 0 AND 5`

### DeliveryAssignmentHistory

PK: Id.

| Поле | Тип | Връзка |
| --- | --- | --- |
| Id | int |  |
| DeliveryId | int | Delivery (Delivery) |
| CourierId | s450 | CourierProfile (Courier) |
| AssignedAtUtc | DateTime |  |
| ReleasedAtUtc | DateTime? |  |
| ReleaseReason | s1000? |  |

Индекси: UNIQUE DeliveryId WHERE [ReleasedAtUtc] IS NULL.

CHECK правила:

- Release: `[ReleasedAtUtc] IS NULL OR [ReleasedAtUtc] >= [AssignedAtUtc]`

### DeliveryStatusHistory

PK: Id.

| Поле | Тип | Връзка |
| --- | --- | --- |
| Id | int |  |
| DeliveryId | int | Delivery (Delivery) |
| FromStatus | DeliveryStatus? |  |
| ToStatus | DeliveryStatus |  |
| ChangedByUserId | s450? | ApplicationUser (ChangedByUser) |
| ChangedAtUtc | DateTime |  |
| Reason | s1000 |  |

Индекси: DeliveryId,ChangedAtUtc.

CHECK правила:

- FromStatus: `[FromStatus] IS NULL OR [FromStatus] BETWEEN 0 AND 5`
- ToStatus: `[ToStatus] BETWEEN 0 AND 5`

### Review

PK: Id.

| Поле | Тип | Връзка |
| --- | --- | --- |
| Id | int |  |
| OrderId | int | Order (Order) |
| RestaurantRating | int |  |
| DeliveryRating | int? |  |
| Comment | s2000? |  |
| CreatedAtUtc | DateTime |  |
| UpdatedAtUtc | DateTime? |  |
| ModerationStatus | ModerationStatus |  |
| RowVersion | byte[] |  |

Индекси: UNIQUE OrderId.

CHECK правила:

- Ratings: `[RestaurantRating] BETWEEN 1 AND 5 AND ([DeliveryRating] IS NULL OR [DeliveryRating] BETWEEN 1 AND 5)`
- ModerationStatus: `[ModerationStatus] BETWEEN 0 AND 1`

### ReviewReply

PK: ReviewId.

| Поле | Тип | Връзка |
| --- | --- | --- |
| ReviewId | int | Review (Review) |
| AuthorUserId | s450 | ApplicationUser (AuthorUser) |
| Text | s2000 |  |
| CreatedAtUtc | DateTime |  |
| UpdatedAtUtc | DateTime? |  |
| ModerationStatus | ModerationStatus |  |

CHECK правила:

- ModerationStatus: `[ModerationStatus] BETWEEN 0 AND 1`

### SupportTicket

PK: Id.

| Поле | Тип | Връзка |
| --- | --- | --- |
| Id | int |  |
| CreatedByUserId | s450 | ApplicationUser (CreatedByUser) |
| OrderId | int? | Order (Order) |
| Category | SupportCategory |  |
| Subject | s200 |  |
| Status | SupportStatus |  |
| AssignedAdminId | s450? | ApplicationUser (AssignedAdmin) |
| CreatedAtUtc | DateTime |  |
| UpdatedAtUtc | DateTime |  |
| RowVersion | byte[] |  |

Индекси: CreatedByUserId,CreatedAtUtc; Status,UpdatedAtUtc.

CHECK правила:

- Category: `[Category] BETWEEN 0 AND 3`
- Status: `[Status] BETWEEN 0 AND 3`

### SupportTicketMessage

PK: Id.

| Поле | Тип | Връзка |
| --- | --- | --- |
| Id | int |  |
| SupportTicketId | int | SupportTicket (SupportTicket) |
| AuthorUserId | s450 | ApplicationUser (AuthorUser) |
| Text | s4000 |  |
| CreatedAtUtc | DateTime |  |

Индекси: SupportTicketId,CreatedAtUtc.

### Notification

PK: Id.

| Поле | Тип | Връзка |
| --- | --- | --- |
| Id | int |  |
| RecipientUserId | s450 | ApplicationUser (RecipientUser) |
| Type | NotificationType |  |
| Title | s200 |  |
| Message | s2000 |  |
| OrderId | int? | Order (Order) |
| SupportTicketId | int? | SupportTicket (SupportTicket) |
| CreatedAtUtc | DateTime |  |
| ReadAtUtc | DateTime? |  |

Индекси: RecipientUserId,CreatedAtUtc; RecipientUserId,CreatedAtUtc WHERE [ReadAtUtc] IS NULL.

CHECK правила:

- Type: `[Type] BETWEEN 0 AND 4`

### AuditLog

PK: Id.

| Поле | Тип | Връзка |
| --- | --- | --- |
| Id | int |  |
| ActorUserId | s450? | ApplicationUser (ActorUser) |
| Action | s100 |  |
| EntityType | s100 |  |
| EntityId | s450 |  |
| CreatedAtUtc | DateTime |  |
| Reason | s1000? |  |
| CorrelationId | s100? |  |

Индекси: EntityType,EntityId,CreatedAtUtc; ActorUserId,CreatedAtUtc.

## Правила за бъдещите услуги (не са реализирани чрез схемата)

- Restaurant глобалната роля не дава достъп до всички ресторанти: изисква се
  активно RestaurantStaff членство с подходящ StaffRole. Аналогично се проверяват
  собственост и активност на профилите. Rejected означава отказано одобрение,
  а не блокиране на вече одобрен участник.
- RestaurantDeliveryArea трябва да е в същия град като Restaurant. Адресният град
  се извежда от DeliveryArea; CustomerAddress няма дублиран CityId.
- ScheduleException замества седмичния график за датата; затворен ден няма
  интервали, няма застъпвания между периодите. CHECK проверява само положителен
  интервал под 24 часа, включително през полунощ.
- Публикувано ястие има поне един активен вариант; без размери се използва
  „Стандартен“. Броят налични опции трябва да позволява min/max selections.
  Една опция се избира веднъж на порция; цената е еднаква за всички варианти.
- Количката е за един ресторант, празната може да няма RestaurantId. Смяната
  изисква потвърждение. Различните конфигурации са отделни позиции; количката
  не пази авторитетни цени, не резервира наличности. Variant/option принадлежността
  и валидността на избора се проверяват от услугата, не чрез cross-table CHECK.
- Checkout е транзакционен: преизчислява наличности, option цени, сборове между
  таблици, минимална сума, район/график/одобрение и snapshots. Един token с различен
  request hash се отхвърля; уникалният token сам не реализира идемпотентен checkout.
  Snapshots не се синхронизират от каталога; няма FK към CustomerAddress.
- Order: New → Accepted/Rejected от ресторанта; New → Cancelled от клиента;
  Accepted → Preparing → Ready от ресторанта; Ready → Completed само при успешно
  доставяне. Проблем след вземане → RequiresResolution. Администраторското
  разрешаване → Failed или Completed само при съгласувани доставка и плащане.
  Enum/CHECK не налагат тези преходи и не проверяват кой ги прави.
- Delivery се създава при приемане на поръчката в бъдеща транзакция. Назначава се
  само активен одобрен свободен куриер в района. При конкурентно приемане Delivery,
  assignment и status histories се записват атомарно. Вземане само при Order.Ready.
  Освобождаване преди вземане → AwaitingCourier и CourierId=NULL, със затворена
  assignment история. След вземане проблемът изисква администратор. Успешното
  приключване обновява Delivery, Order, Payment и histories в една транзакция.
  Уникалните индекси са защитен механизъм, а не реализация на целия процес.
- Payment е един запис на поръчка, CashOnDelivery. Сумата и валутата трябва да
  съвпадат с Order; събраните пари не се заличават административно. DeliveryFee
  не е куриерска заплата. Онлайн плащания/финансово разплащане са отложени.
- Review се създава само от собственика на Completed поръчка. Авторът и ресторантът
  се извеждат от Order; има един официален отговор с проверено staff право.
- Support е достъпен за подател и администратори; връзката към поръчка и AssignedAdmin
  изискват service authorization. Няма attachments. Notification е само в сайта.
- AuditLog и histories се добавят от бъдещите операции. Няма DB механизъм, който
  автоматично ги попълва или забранява всяка директна SQL редакция. Не се записват
  пароли, tokens или чувствителни payload-и. CorrelationId е незадължителен текст.

## Seeding и проверка

DatabaseSeeder е изричен, additive и транзакционен (Serializable). Не прилага миграции.
Reference режимът създава 4 роли, измислен Демоград с 3 района, 3 кухни и 14 алергена
със стабилни кодове. Demo режимът е разрешен от Web само в Development/Testing и
само при Seed:DemoPassword. Без парола се изпълнява само reference частта.
Demo акаунтите имат резервирани IDs dishdrop-demo-* и example.com emails;
конфликт с чужд акаунт се отказва, паролите на съществуващите не се променят.
При зает demo slug всички съвпадения се проверяват преди demo промените.
Изискват се едновременно очакваният град, активно Owner членство на резервирания
demo owner ID, очакваните Identity normalized email/user name и първоначална
seed approval история от резервирания demo admin ID. Чужд активен Owner също
води до отказ. Slug, име, град или demo membership сами по себе си не са
достатъчно доказателство. При непълна принадлежност операцията хвърля ясно
изключение; външната Serializable транзакция отменя и справочните записи.
Не се присвояват чужди ресторанти и не се презаписва съдържание.
Проверката използва съществуващи FK/история, без нови колони или миграция,
и приема валидните demo записи от предишната версия на seeder-а.
Има customer, restaurant owner, employee, courier и admin, два ресторанта,
staff, график, обслужван район/такси, courier profile/район, менюта, категории,
варианти, опции и allergens. Няма demo поръчки. Съществуващ demo каталог не се
презаписва; повторен seed не е инструмент за поправка на ръчно променени данни.

Тестовете използват нови DishDropTests_<guid> бази и изтриват само тези, които
сами са създали. На Windows по подразбиране използват LocalDB; друг SQL Server се
задава с DISHDROP_TEST_SQL. Началната база от connection string се заменя с master
и новото уникално име. Нужни са права CREATE/DROP DATABASE на тестовия сървър.
При липсващ SQL Server тестовете се провалят явно, не се пропускат.
Няма EF InMemory заместител. Category=Database отделя DB тестовете; CI има отделен
job със SQL Server 2022 контейнер и генерирана маскирана парола.

Локално е проверен SQL Server LocalDB 15.0.4382.1: migrations от празна база,
legacy upgrade/duplicate диагностика, уникалности/PK/FK/checks, snapshots/restrict,
active courier/open assignment индекси, rowversion, repeat seeding, SQL script x2,
registration и Identity страници. Подробните крайни бройки са в README.
Docker daemon не беше наличен и GitHub runner не е изпълнен локално.
Схемните тестове не заместват бъдещите service/authorization/workflow тестове.

Официални справки за използваните EF възможности:
[индекси](https://learn.microsoft.com/en-us/ef/core/modeling/indexes),
[миграции](https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/managing),
[Visual Studio PMC](https://learn.microsoft.com/en-us/ef/core/cli/powershell).

## Проверка на организационното рефакториране

Изходно състояние: commit b5a455b, branch feature/database-foundation.
Преди и след замяната на Configuration класовете е експортиран design-time
релационният модел без DB connection: 49 таблици, 326 колони и 44 CHECK ограничения.
Детерминираното сравнение включва имена/schema, типове/nullability, defaults и
computed SQL, collation, PK/unique/FK с колони и delete behavior, индекси с filters,
clustered/include/direction настройки, CHECK SQL, precision/scale/max lengths,
value generation/save behavior, converters и concurrency/shadow свойства.

Експортите преди/след са byte-for-byte еднакви, SHA-256:
556D060629C4631A726836BE6C7953A7A9D78C6B8DC29402E13890269475C987.
Генерираният CREATE SQL също е byte-for-byte еднакъв, SHA-256:
7DB3B5973B8CC22A3DAE3D26B8729A44FBC70A54AEAD568E92ABACEB1E87D9F2.

EF has-pending-model-changes потвърждава съвпадение със запазения snapshot.
Съществуващите migrations, snapshot и предавания SQL script не са редактирани.
Няма нова миграция и няма промяна на приложната база или user-secrets.
SQL Server тестовете използват само новосъздадени disposable тестови бази.

## Традиционен стил и обратни navigation properties

Рефакториране от commit 62aaf7f: namespaces със скоби, традиционен DbContext
конструктор, DbSet get/set, атрибути и using static към DataConstants групите.
Fluent API е директно в OnModelCreating, с отделни chained операции и
описателни lambda имена. Required string initializers са null!; SQL defaults,
nullable полетата, EUR и другите съществуващи initializers не са променени.

Добавени са 67 обратни domain navigation properties и 4 колекции за съществуващите
Identity връзки към ApplicationUser (UserClaims/UserLogins/UserTokens/UserRoles).
Общо: 68 инициализирани ICollection<T> и 3 nullable one-to-one references:
ApplicationUser.CourierProfile, Order.DeliveryAddress и Review.Reply.
При няколко FK към потребител имената са отделни, например
CreatedSupportTickets/AssignedSupportTickets и OrderStatusChanges/DeliveryStatusChanges.
Restaurant.Menus, Menu.Categories, Dish.Variants/OptionGroups, Cart.Items и
останалите обратни връзки използват съществуващите FK и explicit join entities.

Запазена е EF cardinality, включително WithMany при Cart.User, Payment.Order,
Delivery.Order и Review.Order, където SQL unique индекси ограничават броя.
Затова обратните им свойства са колекции (Carts/Payments/Deliveries/Reviews).
Domain NO ACTION и стандартното Identity CASCADE са непроменени.

Преди/след: релационният export отново е byte-for-byte еднакъв, SHA-256
556D060629C4631A726836BE6C7953A7A9D78C6B8DC29402E13890269475C987.
CREATE SQL export: SHA-256
7DB3B5973B8CC22A3DAE3D26B8729A44FBC70A54AEAD568E92ABACEB1E87D9F2.
Запазени са 49 моделни таблици, 326 колони, 73 FK и 44 CHECK ограничения.
Navigation metadata е разширена, релационната схема не е променена.
NavigationMappingTests проверява правилните inverse/FK двойки, различните
user relationships, инициализацията на колекциите и Identity delete behavior,
без отваряне на DB connection.
