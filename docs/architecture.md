# Архитектура

Един solution с пет net10.0 проекта и без циклични зависимости.

| Слой | Отговорност и зависимости |
| --- | --- |
| Web → Core, Data | MVC/Identity UI, HTTP pipeline, конфигурация/DI и изричен seed entry point |
| Core → Data | Бъдещи бизнес правила/use cases; още няма реализирани услуги |
| Data | Entities/enums, ApplicationUser, Fluent configurations, DbContext, migrations и seeder |
| UnitTests → Core | Бъдещи бизнес тестове; засега няма тестове |
| IntegrationTests → Web, Data | HTTP smoke/Identity и SQL Server schema тестове |

Data отделя persistence от HTTP/UI; Core ще отдели бизнес правилата от
контролерите. Persistence enums остават в Data. Всички моделни конфигурации
са в отделни IEntityTypeConfiguration класове и се откриват от контекста.

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
