using InterviewPrep.Domain.Enums;

namespace InterviewPrep.Application.DTOs;

public sealed record InterviewQuestionDto(
    Guid Id,
    string Title,
    string Answer,
    QuestionDifficulty Difficulty,
    string Category,
    bool AdminOnly);

public sealed record CreateInterviewQuestionRequest(
    string Title,
    string Answer,
    QuestionDifficulty Difficulty,
    string Category,
    bool AdminOnly);

public sealed record UpdateInterviewQuestionRequest(
    string Title,
    string Answer,
    QuestionDifficulty Difficulty,
    string Category,
    bool AdminOnly);
