using InterviewPrep.Domain.Common;
using InterviewPrep.Domain.Enums;

namespace InterviewPrep.Domain.Entities;

public sealed class AppUser : BaseEntity
{
    private AppUser()
    {
    }

    public AppUser(string externalId, string displayName, string email, UserRole role)
    {
        ExternalId = externalId;
        DisplayName = displayName;
        Email = email;
        Role = role;
    }

    public string ExternalId { get; private set; } = string.Empty;
    public string DisplayName { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public UserRole Role { get; private set; }

    public void UpdateProfile(string displayName, string email, UserRole role)
    {
        DisplayName = displayName;
        Email = email;
        Role = role;
        MarkUpdated();
    }
}
