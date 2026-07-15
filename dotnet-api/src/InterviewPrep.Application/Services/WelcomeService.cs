using InterviewPrep.Application.DTOs;
using InterviewPrep.Domain.Enums;

namespace InterviewPrep.Application.Services;

public sealed class WelcomeService
{
    public WelcomeDto BuildWelcome(string displayName, UserRole role)
    {
        if (role == UserRole.Admin)
        {
            return new WelcomeDto(
                $"Welcome {displayName}. You are viewing the admin experience.",
                role.ToString(),
                ["User management", "Interview question management", "API health", "Admin reports"]);
        }

        return new WelcomeDto(
            $"Welcome {displayName}. You are viewing the employee experience.",
            role.ToString(),
            ["Personal dashboard", "Practice questions", "Profile"]);
    }
}
