# FinanceHelper

FinanceHelper is a Razor Pages web application built on .NET 10 to help users manage personal finances — income, savings, expense tracking, categories, salary/tax calculations, and simple reporting.

This repository uses a layered architecture with clear separation between domain, application, infrastructure, and web/UI concerns.

Key features
------------
- Create and manage saving accounts and saving transactions
- Track expenses using categories and subcategories
- Capture salary records and calculate taxable amounts
- EF Core persistence with LocalDB / SQL Server support
- Razor Pages UI (startup project `Web`)

Technology
----------
- .NET 10 (C#)
- ASP.NET Core Razor Pages
- Entity Framework Core
- SQL Server / LocalDB (development)
- MSTest for unit tests

Repository layout (high-level)
------------------------------
- `Domain/` — domain entities and enums
- `Application/` — application services, use-cases and interfaces
- `Infrastructure/` — EF Core context, entity configurations and DI wiring
- `Web/` — Razor Pages UI, `appsettings.json`, `Program.cs` (startup)
- `ApplicationTest/` — unit tests

Requirements
------------
- .NET 10 SDK installed: https://dotnet.microsoft.com
- SQL Server or LocalDB for development
- (Optional) `dotnet-ef` for migrations: `dotnet tool install --global dotnet-ef`

Quickstart — development
------------------------
1. Clone the repository:
   `git clone https://github.com/kierenadkins/FinanceHelper.git`
2. Restore and build:
   `dotnet restore`
   `dotnet build`
3. Configure the database connection:
   - Edit `Web/appsettings.json` (or create `appsettings.Development.json`) and set the connection string. Example for LocalDB:
     `"ConnectionStrings": { "FinanceHelperDb": "Server=(localdb)\\mssqllocaldb;Database=FinanceHelperDb;Trusted_Connection=True;MultipleActiveResultSets=true" }`
4. Apply EF Core migrations and update the database (run from solution root):
   - Add migration (if creating new migration):
     `dotnet ef migrations add InitialCreate --project Infrastructure --startup-project Web`
   - Update database:
     `dotnet ef database update --project Infrastructure --startup-project Web`
5. Run the application (startup project is `Web`):
   `dotnet run --project Web`

Run tests
---------
Run unit tests from the solution root:
`dotnet test ApplicationTest`

Development notes
-----------------
- Keep business logic in `Application` and `Domain` layers; `Web` should be a thin UI layer.
- DI configuration is provided in `Application/DependencyInjection.cs` and `Infrastructure/DependencyInjection.cs`.
- Use the solution's existing coding conventions when adding new code.

Contributing
------------
- Fork the repository, create a feature branch, make changes, run tests, and open a PR against `master`.
- Keep PRs small and focused, include a clear description of the change and any migration steps.

Troubleshooting
---------------
- Migration or DB errors: verify the connection string in `Web/appsettings.json` and that SQL Server/LocalDB instance is running.
- Missing EF tools: install `dotnet-ef` as noted above.
- If the app fails at startup, run `dotnet run --project Web` and inspect console logs for details.

License
-------
This project is provided under the MIT license. Update the `LICENSE` file if needed for your organization.

Maintainers / Contact
---------------------
Report issues and feature requests using the repository's GitHub Issues page.
