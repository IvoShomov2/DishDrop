# План за разработка

## Завършени начални етапи

- [x] Един solution с Web, Data, Core, UnitTests и IntegrationTests.
- [x] .NET 10, начална MVC/Identity интеграция и CI.
- [x] Модел за потребители, ресторанти, staff, куриери и райони.
- [x] Менюта, категории, варианти, опции и allergens.
- [x] Количка, поръчки/snapshots, плащания, доставки и histories.
- [x] Отзиви/отговори, поддръжка, известия и audit модел.
- [x] ApplicationUser и адаптирани Register/Manage страници.
- [x] Атрибути и DataConstants плюс Fluent API в DbContext; checks, FK/unique индекси и rowversion.
- [x] Запазена Identity миграция и AddInitialBusinessSchema migration.
- [x] Идемпотентен SQL script, изпитан двукратно.
- [x] Изрично справочно/demo seeding без hardcoded пароли.
- [x] Реални SQL Server LocalDB проверки: 34 DB теста + 4 HTTP и 2 metadata теста без база.
- [x] Традиционен entity/DbContext стил и изрично mapped обратни navigation properties.
- [x] Документация, спецификация и ER диаграми.

Release build: 0 warnings/errors; 40 успешни integration теста, 0 skipped.
UnitTests няма бизнес тестове. Няма твърдение за бизнес coverage.
Локално SQL Server е LocalDB 15.0.4382.1. Docker daemon не беше достъпен;
новият SQL Server 2022 CI job е подготвен, но не е изпълнен в GitHub.
Няма одобрение от консултант и няма реални потребителски/production данни.

## Оставащи задачи

1. Преглед на договорения модел и ръчна проверка на development setup в
   Visual Studio/SSMS; изпълнение на новия GitHub SQL Server job.
2. Потребителски provisioning, authorization по роли/собственост/staff и активност.
3. Бизнес услуги за ресторанти, графици, райони, менюта, наличности и опции.
4. Количка и транзакционен идемпотентен checkout с cross-table проверки.
5. Order/Delivery/Payment workflow и конкурентно приемане с атомарни histories.
6. Review/support/notification/moderation услуги и audit записване.
7. MVC CRUD/потребителски екрани и административни панели.
8. Функционални, authorization и workflow concurrency тестове, ръчна демонстрация
   и подготовка за Етап 1.

Наличните entities/enum стойности/индекси не са реализация на тези услуги.
При бъдещи промени се добавят нови миграции, без изтриване на запазената
Identity история или пресъздаване на бази с реални данни.
