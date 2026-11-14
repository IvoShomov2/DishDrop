# DishDrop

Дипломен проект за доставка на храна с ASP.NET Core MVC, EF Core и SQL Server.
Планирани роли: Customer, Restaurant, Courier и Admin (един потребител може да има няколко).

Текущ статус: многослойна структура и начална бизнес схема с migrations,
изричен seeder и SQL Server integration тестове. Има MVC шаблон и Identity UI;
няма реализирани бизнес услуги, CRUD панели, checkout или куриерски процес.

## Структура

Един solution: DishDrop/DishDrop.slnx. Всички проекти са net10.0.

| Проект | Отговорност |
| --- | --- |
| DishDrop.Web | MVC, Identity Razor Pages, startup/DI и изрични seed команди |
| DishDrop.Data | ApplicationUser, entities/enums, Fluent конфигурации, контекст, migrations и seeder |
| DishDrop.Core | Място за бъдещите бизнес услуги; зависи от Data |
| DishDrop.UnitTests | xUnit проект без бизнес тестове на този етап |
| DishDrop.IntegrationTests | HTTP smoke/Identity проверки и отделени SQL Server schema тестове |

## Изисквания и стартиране

- .NET 10 SDK и достъп до NuGet.
- Visual Studio с .NET 10/.slnx поддръжка и ASP.NET and web development workload.
- SQL Server 2019+ или Windows LocalDB; за Linux тестовете се задава отделен SQL Server.
- Docker е алтернатива за disposable SQL Server; не е нужен при LocalDB.

Командите по-долу са от корена на Git хранилището (папката с README):

~~~powershell
dotnet restore DishDrop/DishDrop.slnx
dotnet build DishDrop/DishDrop.slnx --configuration Release --no-restore
dotnet run --project DishDrop/DishDrop.Web --launch-profile https
~~~

Във Visual Studio отворете DishDrop/DishDrop.slnx, задайте DishDrop.Web като
Startup Project, изберете https и F5. При нужда доверете development HTTPS сертификата.

## Конфигурация и безопасно създаване/upgrade на локална база

Върху DishDrop.Web → Manage User Secrets. Пример с измислено име на нова база:

~~~json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=DishDropDevelopment;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}
~~~

Не презаписвайте други секрети. За съществуваща база първо направете backup и
проверете сървъра/името. Запазен е Web UserSecretsId и LocalDB примерът в appsettings.json.
Production конфигурацията се подава отделно, например чрез environment
ConnectionStrings__DefaultConnection. Пароли/ключове не се записват в Git.

Инсталирайте локалния tool само ако още липсва (папката .tools е игнорирана):

~~~powershell
dotnet tool install dotnet-ef --version 10.0.14 --tool-path .tools
$env:ASPNETCORE_ENVIRONMENT = "Development"
./.tools/dotnet-ef migrations list --project DishDrop/DishDrop.Data --startup-project DishDrop/DishDrop.Web --configuration Release --no-build --no-connect
./.tools/dotnet-ef migrations script --idempotent --project DishDrop/DishDrop.Data --startup-project DishDrop/DishDrop.Web --configuration Release --no-build --output docs/sql/initial-business-schema.sql
~~~

Прегледайте [предавания SQL](docs/sql/initial-business-schema.sql) и
[описанието на миграциите](docs/database.md) преди прилагане. За нова локална
база EF я създава с двете миграции. За база с оригиналната Identity схема
прилага само новата, според __EFMigrationsHistory:

~~~powershell
./.tools/dotnet-ef database update --project DishDrop/DishDrop.Data --startup-project DishDrop/DishDrop.Web --configuration Release --no-build
~~~

Не използвайте EnsureCreated, database drop или изтриване на миграции като setup.
Няма автоматични migrations при startup. Съществуващата Identity миграция и
данните се запазват. Старите имена са празни/неизвестни; CreatedAtUtc при upgrade
е моментът на upgrade, не историческа дата. Дублиран непразен NormalizedEmail
спира миграцията с грешка 51000; не изтривайте/сливайте акаунти автоматично.
Изяснете конфликтите преди повторно прилагане. SQL скриптът няма credentials
или USE и трябва да се изпълни в правилната предварително избрана база.

Във Visual Studio Package Manager Console, с Web като Startup Project:

~~~powershell
Get-Migration -Project DishDrop.Data -StartupProject DishDrop.Web -NoConnect
Script-Migration -Idempotent -Project DishDrop.Data -StartupProject DishDrop.Web -Output docs/sql/initial-business-schema.sql
Update-Database -Project DishDrop.Data -StartupProject DishDrop.Web
~~~

Проверете текущата директория на PMC при относителния Output път.
Identity използва ApplicationUser, стандартни роли, RequireUniqueEmail=true и
запазено RequireConfirmedAccount=true. Реален email provider не е добавян.
Register/Manage са адаптирани само за имената и стандартните полета.

## Изрично seeding

След migrations, с правилния DefaultConnection и Development environment:

~~~powershell
$env:ASPNETCORE_ENVIRONMENT = "Development"
dotnet run --project DishDrop/DishDrop.Web --no-launch-profile -- --seed-reference
~~~

Reference режимът създава ролите, Демоград, три района, кухни и 14 алергена.
Demo режимът изисква парола от user-secrets (заменете placeholder-а локално):

~~~powershell
dotnet user-secrets set "Seed:DemoPassword" "<your-demo-password>" --project DishDrop/DishDrop.Web
dotnet run --project DishDrop/DishDrop.Web --no-launch-profile -- --seed-demo
~~~

Може да използвате environment Seed__DemoPassword вместо secrets, без да го
записвате в скриптове/Git или да го отпечатвате. Паролата трябва да покрива
стандартната Identity password policy. Без стойност demo users не се създават.
Demo режимът се отказва извън Development/Testing; нищо не се seed-ва при обикновен startup.

Demo акаунти: customer@example.com, restaurant@example.com, employee@example.com,
courier@example.com и admin@example.com. Всички са измислени, потвърдени и ползват
зададената demo парола. Двата ресторанта имат собственик/служител, график, район,
такса и менюта с варианти/опции/алергени. Няма demo поръчки.
Повторно изпълнение не дублира записите и не сменя съществуващи пароли.
Резервиран email, принадлежащ на друг акаунт, води до отказ, не до присвояване на роли.

## Тестове и CI

Без база:

~~~powershell
dotnet test DishDrop/DishDrop.slnx --configuration Release --no-build --filter "Category!=Database"
~~~

Всички тестове на Windows LocalDB (тестовете сами създават уникални disposable бази):

~~~powershell
sqllocaldb start MSSQLLocalDB
dotnet test DishDrop/DishDrop.slnx --configuration Release --no-build
~~~

Само SQL Server тестове:

~~~powershell
dotnet test DishDrop/DishDrop.IntegrationTests --configuration Release --no-build --filter "Category=Database"
~~~

За друг изолиран SQL Server задайте DISHDROP_TEST_SQL в средата на тестовия процес
чрез локална секретна конфигурация. Пример без реални credentials:

~~~powershell
$env:DISHDROP_TEST_SQL = "Server=localhost,1433;User ID=<test-login>;Password=<test-password>;TrustServerCertificate=True"
dotnet test DishDrop/DishDrop.IntegrationTests --configuration Release --no-build --filter "Category=Database"
~~~

Не насочвайте тестовете към production/чужд сървър. Нужни са CREATE/DROP DATABASE права.
Тестовете игнорират името на база в настройката, създават DishDropTests_<guid> и
почистват само собствените новосъздадени бази. Не докосват приложната база или
чужди контейнери/volumes. При недостъпен SQL Server DB тестовете се провалят явно.

Локална проверка: restore и Release build с 0 warnings/errors; **38 успешни integration
теста (34 SQL Server, 4 без база), 0 skipped** върху LocalDB 15.0.4382.1.
Проверени са празна база, legacy upgrade със запазен потребител, duplicate email
диагностика, PK/FK/unique/checks, snapshots/restrict delete, filtered indexes,
rowversion, repeat seeding, SQL script x2 и стандартна регистрация.
UnitTests няма тестове — няма заявено бизнес coverage.

CI пази restore/build/smoke job и добавя отделен SQL Server 2022 integration job.
Контейнерната парола се генерира временно и се маскира; job-ът проверява, че DB
тестовете са открити и не са skipped. Няма deployment или coverage threshold.
Docker daemon и GitHub runner не са проверени локално; реалните локални DB
резултати са от SQL Server LocalDB, не от InMemory.

## Ръчна проверка в SSMS

Свържете правилната локална база и прегледайте:
- __EFMigrationsHistory: оригиналната и AddInitialBusinessSchema миграция;
- AspNetUsers: запазени IDs/акаунти, новите колони; стандартните Identity роли;
- ключове, FK с NO ACTION, CHECK constraints и rowversion колони;
- unique filtered индекси за default адрес, активен куриер, open assignment и email;
- справочните/demo данни след изрично seeding и липсата на дубликати при повторение.

Не редактирайте директно историята на поръчки/плащания за функционална демонстрация.
Проверете Home/Privacy, Register/Login и Manage във Visual Studio.
HTTP test host проверките не заместват ръчна браузърна проверка.

## Документация и оставаща работа

- [Архитектура](docs/architecture.md)
- [База, всички полета и ограничения](docs/database.md)
- [ER диаграми по области](docs/database-diagram.md)
- [Спецификация](docs/project-specification.md)
- [План](docs/development-plan.md)

Предстоят бизнес услуги, authorization, транзакционен checkout и courier workflow,
CRUD екрани и панели, модерация/поддръжка и функционални тестове.
SQL ограниченията не реализират тези процеси. Онлайн плащания, GPS и финансово
разплащане с куриери са отложени.
