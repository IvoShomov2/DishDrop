# Архитектура

Един solution с пет net10.0 проекта и без циклични зависимости.

| Слой | Отговорност и зависимости |
| --- | --- |
| Web → Core, Data | MVC/Identity UI, HTTP pipeline, конфигурация/DI и изричен seed entry point |
| Core → Data | Бъдещи бизнес правила/use cases; още няма реализирани услуги |
| Data | Entities/enums с атрибути, ApplicationUser, DataConstants, DbContext, migrations и seeder |
| UnitTests → Core | Бъдещи бизнес тестове; засега няма тестове |
| IntegrationTests → Web, Data | HTTP smoke/Identity и SQL Server schema тестове |

Data отделя persistence от HTTP/UI; Core ще отдели бизнес правилата от
контролерите. Persistence enums остават в Data. Основните настройки са атрибути в entities: Required/MaxLength, Table/Column,
Key/ForeignKey, Timestamp и Precision. DataConstants събира дължините, SQL
типовете и точните CHECK/filter изрази, групирани по entity/област.
Допълнителният Fluent API е директно в ApplicationDbContext.OnModelCreating,
групиран по предметни области след base.OnModelCreating: composite keys, relationships/delete behavior,
индекси, CHECK constraints, defaults и дължини на стандартните Identity ключове.
Няма отделни Configuration класове, Configure помощни методи или автоматично откриване от assembly.
Entities, DataConstants и DbContext използват namespaces със скоби. Контекстът има
традиционен конструктор и DbSet свойства с get/set. В entities има празни редове
между свойствата, null! за required strings/references и using static за
съответните DataConstants групи, когато няма двусмислие. Съществуващите други
initializers (например EUR, IsActive, UTC timestamps и rowversion масиви) са запазени.
Обратните navigation properties са свързани изрично със съществуващите FK.
Колекциите са ICollection<T> с new List<T>(); optional one-to-one обратните
references са nullable. Explicit join entities не са заменени с implicit many-to-many.
Data/Entities остава DB моделът, Web/Models — ViewModels/FormModels, Core — бъдещи услуги.

Identity UI използва Razor Pages и ApplicationUser със запазен string ключ.
Register/Manage са адаптирани за FirstName/LastName, останалото UI е от Identity.
Стандартните роли са Customer/Restaurant/Courier/Admin. Потвърждението на
акаунт остава включено; уникалният email е проверен от Identity и SQL индекс.
Бизнес интерфейсът ще бъде MVC. Authentication е преди authorization;
routing, static assets, HTTPS и обработката на грешки са запазени.

Web остава startup project за EF tooling и собственик на UserSecretsId.
DefaultConnection се чете от конфигурацията; няма embedded credentials.
Design/Tools са PrivateAssets=all. Няма startup migrations или EnsureCreated.
Seeder се изпълнява само с изрична команда, не при обикновен HTTP startup.

Първоначалната Identity миграция е запазена; новата бизнес миграция разширява
схемата и backfill-ва потребителите без изтриване. Бизнес FK използват NO ACTION,
а основните конкурентни записи имат rowversion. Това защитава данните,
без да реализира бъдещите транзакционни workflows.

DB тестовете използват SQL Server и собствени disposable бази.
CI отделя тестовете без база от Category=Database. Наличието на schema тестове
не се отчита като coverage на бъдещи услуги/authorization.

Вижте [database.md](database.md), [ER диаграмите](database-diagram.md) и
[спецификацията](project-specification.md) за подробности.
