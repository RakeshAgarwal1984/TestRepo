using InterviewPrep.Domain.Enums;

namespace InterviewPrep.Application.DTOs;

public sealed record UserDto(
    Guid Id,
    string ExternalId,
    string DisplayName,
    string Email,
    UserRole Role);
