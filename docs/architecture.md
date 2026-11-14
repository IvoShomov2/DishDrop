# Архитектура

DishDrop е в начален структурен етап. Един solution съдържа пет net10.0 проекта.

## Отговорности и зависимости

- Web → Core и Data: MVC контролери/изгледи, HTTP pipeline, конфигурация и DI.
- Core → Data: бъдещи бизнес правила и use cases; засега няма услуги или интерфейси.
- Data: Identity persistence, ApplicationDbContext и съществуващата Identity миграция.
- UnitTests → Core: бъдещи тестове на бизнес логиката; текущо няма тестове.
- IntegrationTests → Web и Data: smoke тест през реалния ASP.NET Core test host.

Няма циклични зависимости. Data не зависи от Web или Core.
Core е отделен, за да не се натрупва бизнес логика в контролерите.
Data отделя EF persistence и миграциите от HTTP/UI слоя.
Това е началната договорена структура, а не реализирана бизнес система.

## MVC и Identity

Бизнес интерфейсът ще бъде MVC. Identity UI използва Razor Pages,
включени чрез AddDefaultIdentity и MapRazorPages.
Използва се IdentityUser със запазено RequireConfirmedAccount = true.
Authentication е преди authorization; MVC routing, static assets,
HTTPS и обработката на грешки са запазени.

ApplicationDbContext и миграциите са в DishDrop.Data.
Migration assembly е зададен чрез assembly-то на контекста.
Запазени са ID 00000000000000_CreateIdentitySchema, Identity схемата,
migration/designer partial класовете и ModelSnapshot.
Web остава startup проект за EF tooling и притежава UserSecretsId.

ConnectionStrings:DefaultConnection идва от конфигурацията.
Design и Tools зависимостите в Web са PrivateAssets=all.
Няма автоматично прилагане на миграции и няма design-time factory
с вградени credentials.

## Проверки

Integration smoke тестът използва WebApplicationFactory, HTTPS адрес,
GET / и Welcome заглавието. EF interceptor прекъсва всеки опит за
отваряне на DB connection. Authentication и authorization остават включени.
Тестът не доказва работа на Identity срещу SQL Server.

CI проверява компилация и тестове без база. Реална SQL Server проверка
и проверка на бизнес модела предстоят.
