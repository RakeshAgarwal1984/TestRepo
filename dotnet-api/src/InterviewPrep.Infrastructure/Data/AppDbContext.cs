using InterviewPrep.Domain.Entities;
using InterviewPrep.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace InterviewPrep.Infrastructure.Data;

public sealed class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<AppUser> Users => Set<AppUser>();
    public DbSet<InterviewQuestion> InterviewQuestions => Set<InterviewQuestion>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AppUser>(entity =>
        {
            entity.ToTable("users");
            entity.HasKey(user => user.Id);
            entity.Property(user => user.ExternalId).HasMaxLength(200).IsRequired();
            entity.Property(user => user.DisplayName).HasMaxLength(160).IsRequired();
            entity.Property(user => user.Email).HasMaxLength(256).IsRequired();
            entity.Property(user => user.Role).HasConversion<string>().HasMaxLength(40).IsRequired();
            entity.HasIndex(user => user.ExternalId).IsUnique();
            entity.HasIndex(user => user.Email).IsUnique();
        });

        modelBuilder.Entity<InterviewQuestion>(entity =>
        {
            entity.ToTable("interview_questions");
            entity.HasKey(question => question.Id);
            entity.Property(question => question.Title).HasMaxLength(220).IsRequired();
            entity.Property(question => question.Answer).HasMaxLength(4000).IsRequired();
            entity.Property(question => question.Category).HasMaxLength(120).IsRequired();
            entity.Property(question => question.Difficulty).HasConversion<string>().HasMaxLength(40).IsRequired();
            entity.HasIndex(question => question.Category);
        });

        Seed(modelBuilder);
    }

    private static void Seed(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AppUser>().HasData(
            new
            {
                Id = Guid.Parse("7d1a9926-d7a6-495a-b9de-1d04c7a6b621"),
                CreatedUtc = DateTime.SpecifyKind(new DateTime(2026, 1, 1), DateTimeKind.Utc),
                UpdatedUtc = (DateTime?)null,
                ExternalId = "admin-demo",
                DisplayName = "Avery Admin",
                Email = "avery.admin@contoso.com",
                Role = UserRole.Admin
            },
            new
            {
                Id = Guid.Parse("3b8dd88d-7f0d-48fb-bf35-a1a49cf5ff29"),
                CreatedUtc = DateTime.SpecifyKind(new DateTime(2026, 1, 1), DateTimeKind.Utc),
                UpdatedUtc = (DateTime?)null,
                ExternalId = "employee-demo",
                DisplayName = "Emery Employee",
                Email = "emery.employee@contoso.com",
                Role = UserRole.Employee
            });

        modelBuilder.Entity<InterviewQuestion>().HasData(
            new
            {
                Id = Guid.Parse("87a88b4e-db97-43fb-894e-bbb1a830005d"),
                CreatedUtc = DateTime.SpecifyKind(new DateTime(2026, 1, 1), DateTimeKind.Utc),
                UpdatedUtc = (DateTime?)null,
                Title = "What is dependency injection?",
                Answer = "Dependency injection provides a class its dependencies from the outside, improving testability and loose coupling.",
                Difficulty = QuestionDifficulty.Easy,
                Category = ".NET",
                AdminOnly = false
            },
            new
            {
                Id = Guid.Parse("fdf74caf-ef48-4c08-b4f5-c32865586e81"),
                CreatedUtc = DateTime.SpecifyKind(new DateTime(2026, 1, 1), DateTimeKind.Utc),
                UpdatedUtc = (DateTime?)null,
                Title = "Explain EF Core change tracking.",
                Answer = "EF Core tracks entity instances loaded into a DbContext and uses their state to generate database changes on SaveChanges.",
                Difficulty = QuestionDifficulty.Medium,
                Category = "Entity Framework",
                AdminOnly = false
            },
            new
            {
                Id = Guid.Parse("6a342797-5ed4-4b7e-810d-7d11aa07dff9"),
                CreatedUtc = DateTime.SpecifyKind(new DateTime(2026, 1, 1), DateTimeKind.Utc),
                UpdatedUtc = (DateTime?)null,
                Title = "How would you secure role-based admin endpoints?",
                Answer = "Validate JWTs, map role claims, apply authorization policies, and enforce critical authorization rules in the API.",
                Difficulty = QuestionDifficulty.Hard,
                Category = "Security",
                AdminOnly = true
            });
    }
}
