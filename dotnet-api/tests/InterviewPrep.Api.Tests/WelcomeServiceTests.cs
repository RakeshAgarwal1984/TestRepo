using InterviewPrep.Application.Services;
using InterviewPrep.Domain.Enums;
using Xunit;

namespace InterviewPrep.Api.Tests;

public sealed class WelcomeServiceTests
{
    [Fact]
    public void BuildWelcome_ForAdmin_ReturnsAdminFeatures()
    {
        var service = new WelcomeService();

        var result = service.BuildWelcome("Avery", UserRole.Admin);

        Assert.Equal("Admin", result.Role);
        Assert.Contains("User management", result.VisibleFeatures);
    }

    [Fact]
    public void BuildWelcome_ForEmployee_ReturnsEmployeeFeatures()
    {
        var service = new WelcomeService();

        var result = service.BuildWelcome("Emery", UserRole.Employee);

        Assert.Equal("Employee", result.Role);
        Assert.Contains("Practice questions", result.VisibleFeatures);
    }
}
