namespace InterviewPrep.Domain.Common;

public abstract class BaseEntity
{
    public Guid Id { get; protected set; } = Guid.NewGuid();
    public DateTime CreatedUtc { get; protected set; } = DateTime.UtcNow;
    public DateTime? UpdatedUtc { get; protected set; }

    public void MarkUpdated()
    {
        UpdatedUtc = DateTime.UtcNow;
    }
}
