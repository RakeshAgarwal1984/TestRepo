using InterviewPrep.Domain.Common;
using InterviewPrep.Domain.Enums;

namespace InterviewPrep.Domain.Entities;

public sealed class InterviewQuestion : BaseEntity
{
    private InterviewQuestion()
    {
    }

    public InterviewQuestion(string title, string answer, QuestionDifficulty difficulty, string category, bool adminOnly)
    {
        Title = title;
        Answer = answer;
        Difficulty = difficulty;
        Category = category;
        AdminOnly = adminOnly;
    }

    public string Title { get; private set; } = string.Empty;
    public string Answer { get; private set; } = string.Empty;
    public QuestionDifficulty Difficulty { get; private set; }
    public string Category { get; private set; } = string.Empty;
    public bool AdminOnly { get; private set; }

    public void Update(string title, string answer, QuestionDifficulty difficulty, string category, bool adminOnly)
    {
        Title = title;
        Answer = answer;
        Difficulty = difficulty;
        Category = category;
        AdminOnly = adminOnly;
        MarkUpdated();
    }
}
