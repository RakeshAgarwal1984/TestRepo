using InterviewPrep.Application.Abstractions;
using InterviewPrep.Application.DTOs;
using InterviewPrep.Domain.Entities;
using InterviewPrep.Domain.Enums;

namespace InterviewPrep.Application.Services;

public sealed class InterviewQuestionService
{
    private readonly IInterviewQuestionRepository _repository;

    public InterviewQuestionService(IInterviewQuestionRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyCollection<InterviewQuestionDto>> ListVisibleAsync(UserRole role, CancellationToken cancellationToken)
    {
        var questions = await _repository.ListVisibleAsync(role, cancellationToken);
        return questions.Select(ToDto).ToArray();
    }

    public async Task<InterviewQuestionDto?> GetByIdAsync(Guid id, UserRole role, CancellationToken cancellationToken)
    {
        var question = await _repository.GetByIdAsync(id, cancellationToken);

        if (question is null || question.AdminOnly && role != UserRole.Admin)
        {
            return null;
        }

        return ToDto(question);
    }

    public async Task<InterviewQuestionDto> CreateAsync(CreateInterviewQuestionRequest request, CancellationToken cancellationToken)
    {
        var question = new InterviewQuestion(
            request.Title,
            request.Answer,
            request.Difficulty,
            request.Category,
            request.AdminOnly);

        await _repository.AddAsync(question, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        return ToDto(question);
    }

    public async Task<InterviewQuestionDto?> UpdateAsync(Guid id, UpdateInterviewQuestionRequest request, CancellationToken cancellationToken)
    {
        var question = await _repository.GetByIdAsync(id, cancellationToken);

        if (question is null)
        {
            return null;
        }

        question.Update(request.Title, request.Answer, request.Difficulty, request.Category, request.AdminOnly);
        await _repository.SaveChangesAsync(cancellationToken);

        return ToDto(question);
    }

    private static InterviewQuestionDto ToDto(InterviewQuestion question)
    {
        return new InterviewQuestionDto(
            question.Id,
            question.Title,
            question.Answer,
            question.Difficulty,
            question.Category,
            question.AdminOnly);
    }
}
