using InterviewPrep.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace InterviewPrep.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<InterviewQuestionService>();
        services.AddScoped<WelcomeService>();

        return services;
    }
}
