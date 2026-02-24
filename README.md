# TaskDigitalHub

Project Management API built with .NET 8, CQRS, Clean Architecture, SignalR, Hangfire, and JWT authentication.

## Tech Stack

- .NET 8 | ASP.NET Core Web API | EF Core | SQL Server
- CQRS + MediatR | FluentValidation | Hangfire | SignalR
- JWT Authentication | Role-based Authorization (Admin, ProjectManager, Developer)

## Prerequisites

- [.NET 8 SDK]
- SQL Server (LocalDB, SQL Server Express, or full SQL Server)

## Setup

### 1. Clone & Restore

```bash
git clone <repository-url>
cd TaskDigitalhub
dotnet restore
```

### 2. Configure Database

Update `TaskDigitalHub/appsettings.json` with your connection string:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=TaskDigitalHubDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
  }
}
```

For SQL Server: `Server=localhost;Database=TaskDigitalHubDb;User Id=sa;Password=YourPassword;TrustServerCertificate=True`

### 3. Configure JWT (Optional)

Update `appsettings.json`:

```json
{
  "Jwt": {
    "Secret": "YourSuperSecretKeyForJwtTokenGeneration_MustBeAtLeast32Characters!",
    "Issuer": "TaskDigitalHub",
    "Audience": "TaskDigitalHub",
    "ExpirationInMinutes": 60
  }
}
```

### 4. Run Migrations

```bash
dotnet ef database update --project TaskDigitalhub.Infrastructure --startup-project TaskDigitalHub
```


## Run

```bash
cd TaskDigitalHub
dotnet run
```

- **API:** https://localhost:7109  
- **Swagger:** https://localhost:7109/swagger  
- **Hangfire Dashboard:** https://localhost:7109/hangfire  
- **SignalR Test Page:** https://localhost:7109/signalr-test.html  

### API Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/api/auth/register` | Register user |
| POST | `/api/auth/login` | Login (returns JWT) |
| GET/POST/PUT/DELETE | `/api/projects` | Projects CRUD |
| GET/POST/PUT/DELETE | `/api/tasks` | Tasks CRUD |
| PATCH | `/api/tasks/bulk-status` | Bulk status update |


## Project Structure

```
TaskDigitalhub/
├── TaskDigitalHub/           # API (Controllers, Hubs, Middleware)
├── TaskDigitalhub.Application/  # CQRS, DTOs, Validators, Interfaces
├── TaskDigitalhub.Domain/       # Entities, Enums
└── TaskDigitalhub.Infrastructure/ # EF, Repositories, Hangfire, Services
```
