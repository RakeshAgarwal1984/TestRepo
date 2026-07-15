namespace InterviewPrep.Application.DTOs;

public sealed record WelcomeDto(
    string Message,
    string Role,
    IReadOnlyCollection<string> VisibleFeatures);
