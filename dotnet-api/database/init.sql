create table if not exists users (
    "Id" uuid primary key,
    "CreatedUtc" timestamp with time zone not null,
    "UpdatedUtc" timestamp with time zone null,
    "ExternalId" varchar(200) not null,
    "DisplayName" varchar(160) not null,
    "Email" varchar(256) not null,
    "Role" varchar(40) not null
);

create unique index if not exists "IX_users_ExternalId"
    on users ("ExternalId");

create unique index if not exists "IX_users_Email"
    on users ("Email");

create table if not exists interview_questions (
    "Id" uuid primary key,
    "CreatedUtc" timestamp with time zone not null,
    "UpdatedUtc" timestamp with time zone null,
    "Title" varchar(220) not null,
    "Answer" varchar(4000) not null,
    "Difficulty" varchar(40) not null,
    "Category" varchar(120) not null,
    "AdminOnly" boolean not null
);

create index if not exists "IX_interview_questions_Category"
    on interview_questions ("Category");

insert into users (
    "Id",
    "CreatedUtc",
    "UpdatedUtc",
    "ExternalId",
    "DisplayName",
    "Email",
    "Role"
)
values
    (
        '7d1a9926-d7a6-495a-b9de-1d04c7a6b621',
        '2026-01-01 00:00:00+00',
        null,
        'admin-demo',
        'Avery Admin',
        'avery.admin@contoso.com',
        'Admin'
    ),
    (
        '3b8dd88d-7f0d-48fb-bf35-a1a49cf5ff29',
        '2026-01-01 00:00:00+00',
        null,
        'employee-demo',
        'Emery Employee',
        'emery.employee@contoso.com',
        'Employee'
    )
on conflict ("Id") do update set
    "ExternalId" = excluded."ExternalId",
    "DisplayName" = excluded."DisplayName",
    "Email" = excluded."Email",
    "Role" = excluded."Role";

insert into interview_questions (
    "Id",
    "CreatedUtc",
    "UpdatedUtc",
    "Title",
    "Answer",
    "Difficulty",
    "Category",
    "AdminOnly"
)
values
    (
        '87a88b4e-db97-43fb-894e-bbb1a830005d',
        '2026-01-01 00:00:00+00',
        null,
        'What is dependency injection?',
        'Dependency injection provides a class its dependencies from the outside, improving testability and loose coupling.',
        'Easy',
        '.NET',
        false
    ),
    (
        'fdf74caf-ef48-4c08-b4f5-c32865586e81',
        '2026-01-01 00:00:00+00',
        null,
        'Explain EF Core change tracking.',
        'EF Core tracks entity instances loaded into a DbContext and uses their state to generate database changes on SaveChanges.',
        'Medium',
        'Entity Framework',
        false
    ),
    (
        '6a342797-5ed4-4b7e-810d-7d11aa07dff9',
        '2026-01-01 00:00:00+00',
        null,
        'How would you secure role-based admin endpoints?',
        'Validate JWTs, map role claims, apply authorization policies, and enforce critical authorization rules in the API.',
        'Hard',
        'Security',
        true
    ),
    (
        '52c61033-4431-45cc-89a1-2f7b186adba1',
        '2026-01-01 00:00:00+00',
        null,
        'What is middleware in ASP.NET Core?',
        'Middleware components run in order as part of the HTTP request pipeline and can inspect, short-circuit, or pass requests to the next component.',
        'Medium',
        'ASP.NET Core',
        false
    ),
    (
        'db3ad503-dd06-4ed9-af8b-301fc2f5ce24',
        '2026-01-01 00:00:00+00',
        null,
        'What is the repository pattern?',
        'The repository pattern hides persistence details behind an interface so application services can work with domain concepts instead of database APIs.',
        'Medium',
        'Architecture',
        false
    ),
    (
        'f27234b3-ebf6-4cc7-b119-6274b8248107',
        '2026-01-01 00:00:00+00',
        null,
        'How do you design an admin-only API action?',
        'Require authentication, validate role claims with an authorization policy, check business rules in the application layer, and log sensitive changes.',
        'Hard',
        'Security',
        true
    )
on conflict ("Id") do update set
    "Title" = excluded."Title",
    "Answer" = excluded."Answer",
    "Difficulty" = excluded."Difficulty",
    "Category" = excluded."Category",
    "AdminOnly" = excluded."AdminOnly";
