# InterviewPrep .NET API

Layered .NET 8 Web API for interview practice.

## Layers

- `InterviewPrep.Api`: controllers, authentication, authorization, CORS, Swagger.
- `InterviewPrep.Application`: DTOs, services, repository interfaces.
- `InterviewPrep.Domain`: entities, enums, core business model.
- `InterviewPrep.Infrastructure`: EF Core DbContext, PostgreSQL repositories, seed data.
- `InterviewPrep.Api.Tests`: xUnit tests.

## Local PostgreSQL

Default connection string:

```text
Host=localhost;Port=5432;Database=interview_prep;Username=postgres;Password=postgres
```

Update it in:

```text
src/InterviewPrep.Api/appsettings.Development.json
```

Create the database if it does not exist:

```bash
createdb interview_prep
```

If your PostgreSQL username/password is different, update the connection string before running migrations.

You can also create the tables and seed interview sample data directly with:

```bash
PGPASSWORD=admin psql -h localhost -p 5432 -U postgres -d test -f database/init.sql
```

This creates:

- `users`
- `interview_questions`

Seed data includes one `Admin`, one `Employee`, and six interview questions.

## Run In VS Code

1. Install the .NET 8 SDK.
2. Open this folder in VS Code:

```text
/Users/akashagarwal/Documents/TestProject/dotnet-api
```

3. Restore packages:

```bash
dotnet restore
```

4. Install the EF Core CLI tool if needed:

```bash
dotnet tool install --global dotnet-ef
```

5. Create the first migration:

```bash
dotnet ef migrations add InitialCreate \
  --project src/InterviewPrep.Infrastructure \
  --startup-project src/InterviewPrep.Api \
  --output-dir Data/Migrations
```

6. Apply it to PostgreSQL:

```bash
dotnet ef database update \
  --project src/InterviewPrep.Infrastructure \
  --startup-project src/InterviewPrep.Api
```

7. Run the API:

```bash
dotnet run --project src/InterviewPrep.Api
```

Open Swagger:

```text
http://localhost:5058/swagger
```

The API does not run database migrations automatically by default. This keeps Swagger available even when PostgreSQL is not started yet.

## Useful Endpoints

- `GET /api/health`
- `GET /api/welcome`
- `GET /api/interview-questions`
- `POST /api/interview-questions` for Admin users
- `PUT /api/interview-questions/{id}` for Admin users

The secured endpoints expect a JWT bearer token with a role claim of either `Admin` or `Employee`.
