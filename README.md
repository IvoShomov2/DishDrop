# DishDrop

Дипломен проект: ASP.NET Core MVC приложение за доставка на храна.
Планирани роли: Customer, Restaurant, Courier и Admin.

Текущ статус: начална многослойна структура с MVC шаблон и Identity,
а не готов продукт. Няма бизнес entities, услуги или реализирани поръчки.

## Проекти

Един solution: DishDrop/DishDrop.slnx, всички проекти са net10.0.

| Проект | Отговорност |
| --- | --- |
| DishDrop.Web | MVC, Identity Razor Pages, startup и DI |
| DishDrop.Data | ApplicationDbContext, Identity persistence и съществуващи миграции |
| DishDrop.Core | Бъдеща бизнес логика; засега само библиотека с reference към Data |
| DishDrop.UnitTests | xUnit проект без бизнес тестове на този етап |
| DishDrop.IntegrationTests | WebApplicationFactory smoke тест на началната страница |

## Изисквания и стартиране

- .NET 10 SDK.
- Visual Studio с поддръжка на .NET 10 и .slnx и workload ASP.NET and web development.
- SQL Server или LocalDB за реалните Identity операции. Smoke тестът не изисква база.
- Достъп до NuGet при първоначален restore.

Команди от корена на Git хранилището (папката с този README):

~~~powershell
dotnet restore DishDrop/DishDrop.slnx
dotnet build DishDrop/DishDrop.slnx --configuration Release --no-restore
dotnet test DishDrop/DishDrop.slnx --configuration Release --no-build
dotnet run --project DishDrop/DishDrop.Web --launch-profile https
~~~

Във Visual Studio отворете DishDrop/DishDrop.slnx, задайте DishDrop.Web
като Startup Project, изберете https и стартирайте с F5.
При нужда доверете локалния development HTTPS сертификат.

## DefaultConnection и Identity

Върху DishDrop.Web: десен бутон → Manage User Secrets. Задайте:

~~~json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=DishDrop;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}
~~~

Използвайте своята съществуваща база, ако вече имате такава; примерът
не създава база. За SQL authentication задавайте реалните credentials
само в User Secrets или environment configuration, никога в Git.
Ключът е ConnectionStrings:DefaultConnection; празна/липсваща стойност
предизвиква ясна startup грешка. Съществуващият LocalDB пример в
appsettings.json и UserSecretsId в Web са запазени.

Identity използва IdentityUser и RequireConfirmedAccount = true.
Не са добавени роли или ApplicationUser. Приложението не прилага миграции
автоматично. Тази задача не създава и не обновява база.

## EF tooling без връзка към база

Startup project е Web, target project е Data. За локална инсталация:

~~~powershell
dotnet tool install dotnet-ef --version 10.0.14 --tool-path .tools
./.tools/dotnet-ef dbcontext list --project DishDrop/DishDrop.Data --startup-project DishDrop/DishDrop.Web --configuration Release --no-build
./.tools/dotnet-ef migrations list --no-connect --project DishDrop/DishDrop.Data --startup-project DishDrop/DishDrop.Web --configuration Release --no-build
~~~

.tools е игнорирана локална папка. Изброяването с --no-connect не показва
дали миграцията е приложена. Не изпълнявайте database update като част
от тези проверки.

## Проверки и CI

CI изпълнява restore, Release build и tests при push към main,
pull request към main и ръчно стартиране. Няма SQL service или deployment.
Smoke тестът проверява GET / и Welcome заглавието от шаблона; interceptor
забранява връзките към база, без да изключва authentication/authorization.
UnitTests няма тестове и не показва бизнес coverage.

Локално са успешни restore, Release build (0 warnings/errors), един
integration тест, откриване на DbContext и изброяване на миграцията с
--no-connect. Реални SQL Server операции, вход/регистрация в браузър и
GitHub Actions runner не са проверявани.

Съществуващите EF/Identity версии 10.0.14 са запазени. Data задава изрично
System.Security.Cryptography.Pkcs 10.0.14, защото restore откри NU1902/NU1903
за транзитивната 9.0.11 от SQL Server зависимостите. След override-а тези
предупреждения не се възпроизвеждат при локален restore/build.

## Предстои

Ресторанти, менюта, варианти и добавки, райони, количка, поръчки,
доставки, плащане в брой, отзиви, запитвания и известия в сайта.

Вижте [архитектурата](docs/architecture.md) и
[плана за разработка](docs/development-plan.md).
