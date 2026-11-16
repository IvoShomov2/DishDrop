# Архитектура и организация на кода

Архитектурата остава ASP.NET Core MVC с Razor, EF Core и SQL Server, организирана в Web/Core/Data и два тестови проекта. Новият продуктов обхват е в [спецификацията](project-specification.md). Примерните пътища по-долу не са възлагане за генериране на код.

## Solution и зависимости

Един solution: `DishDrop/DishDrop.slnx`. Всички настоящи проекти са `net10.0`.

| Проект | Зависимости | Отговорност |
| --- | --- | --- |
| DishDrop.Web | Core, Data | HTTP, MVC/Identity UI, startup/DI и изрични seed команди |
| DishDrop.Core | Data | Бизнес услуги, интерфейси и модели за услугите при необходимост |
| DishDrop.Data | Няма project references | DB entities, enums, константи, контекст, миграции и seeding |
| DishDrop.UnitTests | Core | Тестове на бизнес правилата; още няма такива тестове |
| DishDrop.IntegrationTests | Web, Data | HTTP/Identity, EF metadata и отделени SQL Server проверки |

Core не зависи от Web. Няма циклични зависимости. Директният Web → Data reference остава за Identity и DI; той не разрешава бизнес контролерите да изпълняват заявки към DbContext.

## Web

Пътищата са спрямо `DishDrop/DishDrop.Web/`.

| Папка/файл | Употреба | Фактическо състояние |
| --- | --- | --- |
| Controllers/ | HTTP заявки, ModelState, извикване на услуги и избор на отговор/изглед | Само HomeController.cs |
| Models/ | MVC ViewModels и FormModels, отделени от entities | Само ErrorViewModel.cs |
| Views/ | Показване на подадените модели; без бизнес заявки | Home и Shared шаблонни изгледи |
| Areas/ | Identity Razor Pages и области при конкретно възлагане | Адаптирани Register и Manage/Index; останалият Identity UI е от библиотеката |
| ViewComponents/ | Преизползваеми UI компоненти при конкретна нужда | Планирано място; папка/компоненти още няма |
| wwwroot/ | CSS, JavaScript, изображения и други static assets | Съществува |
| Program.cs | DishDrop.Web.Program.Main, DI, middleware, маршрути, изрично seeding | Традиционен async Main |

Razor PageModels имат собствени вложени InputModel класове в Areas/Identity. Те са отделени от DB entities. Бъдещите MVC модели остават в Web/Models и не се местят в Data.

Контролерът обработва HTTP и извиква услуга. Бизнес правилата, проверките на собственост/активност и заявките към ApplicationDbContext са в услугата. Views визуализират моделите и не извършват бизнес промени. Това е договореният подход за бъдещите бизнес екрани, а не твърдение, че вече има такива.

## Core

Пътищата са спрямо `DishDrop/DishDrop.Core/`. Засега проектът съдържа само csproj: няма Services, модели за услуги или бизнес реализации.

При отделно възлагане интерфейсът и реализацията са заедно по функционалност, например:

```text
Services/
└── Restaurants/
    ├── IRestaurantService.cs
    └── RestaurantService.cs
```

Ако услугата има нужда от входни/изходни модели, те са в Core, групирани към съответната функционалност. Те не използват MVC ViewModels/FormModels от Web. Не се създават празни Services, интерфейси или общи базови класове предварително.

## Data

Реални пътища спрямо `DishDrop/DishDrop.Data/`:

- `Entities/` — текущият DB модел, включително ApplicationUser.
- `Enums/` — persistence enums при нужда; числовите стойности се променят само с план за съществуващите записи.
- `DataConstants.cs` — дължини, SQL типове, CHECK/filter изрази по област/entity.
- `ApplicationDbContext.cs` — IdentityDbContext<ApplicationUser>, DbSet свойства и Fluent API директно в OnModelCreating.
- `Migrations/` — съществуващите Identity и бизнес миграции със snapshot/designer файловете.
- `Seeding/DatabaseSeeder.cs` — отделно, изрично стартирано справочно/demo seeding.
- `RoleNames.cs` — техническите имена Customer, Restaurant, Courier и Admin.

Entities използват приложимите Required/MaxLength, Table/Key/ForeignKey, Column/Precision и Timestamp атрибути с DataConstants. Composite keys, relationships/delete behavior, defaults, уникални/filtered индекси и CHECK constraints са директно в OnModelCreating, групирани с кратки коментари след `base.OnModelCreating(builder)`. Няма отделни Configuration класове или assembly scanning. Range не замества SQL CHECK.

Namespaces са със скоби; конструкторите са традиционни; свойствата са get/set с празни редове. Required strings/reference navigations използват `null!`, optional полетата са nullable, колекциите са `ICollection<T>` с `new List<T>()`. Using static към вложените DataConstants класове се използва без двусмислие. Не се променят други initializers/defaults само за оформление. Част от наследените Web/test файлове още имат по-компактен стил; тази документационна задача не ги преформатира.

## Текущ код и предстоящи промени

Текущият модел е по-широк от новата начална версия: има staff, Menu, варианти/опции, различни такси по райони, отзиви, поддръжка, известия и AuditLog. Реалната схема остава в [database.md](database.md) и [ER диаграмите](database-diagram.md). Опростяването е само [планирано](database-change-plan.md).

В Program.cs още са включени RequireConfirmedAccount и RequireUniqueEmail; Register не дава Customer роля, а IsActive няма завършен механизъм за блокиране/прекратяване на сесии. Новият договор изисква промени в отделни реализационни задачи. Identity настройки не се променят с настоящата документация.

Web е startup project за EF tooling и притежава UserSecretsId. DefaultConnection идва от конфигурацията; appsettings.json няма локален connection string. Няма startup migrations или EnsureCreated. Seeder-ът е opt-in; demo режимът е само за Development/Testing.

SQL тестовете използват собствените disposable бази по SqlTestDatabase. HTTP тестовете използват TestApplicationFactory с ранна host конфигурация и Testing без User Secrets. CI има отделни jobs за тестовете без база и SQL Server. Стар резултат не е текуща проверка. За разграничението между техническа основа и бъдещи услуги вижте [плана](development-plan.md).
