using InterviewPrep.Domain.Entities;

namespace InterviewPrep.Application.Abstractions;

public interface IUserRepository
{
    Task<AppUser?> GetByExternalIdAsync(string externalId, CancellationToken cancellationToken);
    Task AddAsync(AppUser user, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
