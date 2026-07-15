using InterviewPrep.Domain.Entities;
using InterviewPrep.Domain.Enums;

namespace InterviewPrep.Application.Abstractions;

public interface IInterviewQuestionRepository
{
    Task<IReadOnlyCollection<InterviewQuestion>> ListVisibleAsync(UserRole role, CancellationToken cancellationToken);
    Task<InterviewQuestion?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task AddAsync(InterviewQuestion question, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
