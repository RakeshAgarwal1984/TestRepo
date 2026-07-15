using InterviewPrep.Application.Abstractions;
using InterviewPrep.Domain.Entities;
using InterviewPrep.Domain.Enums;
using InterviewPrep.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace InterviewPrep.Infrastructure.Repositories;

public sealed class InterviewQuestionRepository : IInterviewQuestionRepository
{
    private readonly AppDbContext _dbContext;

    public InterviewQuestionRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyCollection<InterviewQuestion>> ListVisibleAsync(UserRole role, CancellationToken cancellationToken)
    {
        return await _dbContext.InterviewQuestions
            .AsNoTracking()
            .Where(question => role == UserRole.Admin || !question.AdminOnly)
            .OrderBy(question => question.Category)
            .ThenBy(question => question.Difficulty)
            .ToArrayAsync(cancellationToken);
    }

    public Task<InterviewQuestion?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return _dbContext.InterviewQuestions.FirstOrDefaultAsync(question => question.Id == id, cancellationToken);
    }

    public async Task AddAsync(InterviewQuestion question, CancellationToken cancellationToken)
    {
        await _dbContext.InterviewQuestions.AddAsync(question, cancellationToken);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        return _dbContext.SaveChangesAsync(cancellationToken);
    }
}
