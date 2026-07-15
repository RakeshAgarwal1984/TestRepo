using InterviewPrep.Application.Abstractions;
using InterviewPrep.Domain.Entities;
using InterviewPrep.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace InterviewPrep.Infrastructure.Repositories;

public sealed class UserRepository : IUserRepository
{
    private readonly AppDbContext _dbContext;

    public UserRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<AppUser?> GetByExternalIdAsync(string externalId, CancellationToken cancellationToken)
    {
        return _dbContext.Users.FirstOrDefaultAsync(user => user.ExternalId == externalId, cancellationToken);
    }

    public async Task AddAsync(AppUser user, CancellationToken cancellationToken)
    {
        await _dbContext.Users.AddAsync(user, cancellationToken);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        return _dbContext.SaveChangesAsync(cancellationToken);
    }
}
