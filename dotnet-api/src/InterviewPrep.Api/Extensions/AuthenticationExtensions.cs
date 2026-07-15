using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace InterviewPrep.Api.Extensions;

public static class AuthenticationExtensions
{
    public static IServiceCollection AddApiAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var authority = configuration["Authentication:Authority"];
        var audience = configuration["Authentication:Audience"];
        var requiredScopes = configuration
            .GetSection("Authentication:RequiredScopes")
            .GetChildren()
            .Select(scope => scope.Value)
            .Where(scope => !string.IsNullOrWhiteSpace(scope))
            .ToArray();

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = authority;
                options.Audience = audience;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = ClaimTypes.Name,
                    RoleClaimType = ClaimTypes.Role,
                    ValidateAudience = !string.IsNullOrWhiteSpace(audience)
                };
            });

        services.AddAuthorization(options =>
        {
            options.AddPolicy("AdminOnly", policy =>
                policy.RequireAuthenticatedUser()
                    .RequireAssertion(context =>
                        context.User.TryGetRole(out var role) && role == Domain.Enums.UserRole.Admin));

            options.AddPolicy("EmployeeOrAdmin", policy =>
                policy.RequireAuthenticatedUser()
                    .RequireAssertion(context =>
                    {
                        var hasRole = context.User.TryGetRole(out var role)
                            && role is Domain.Enums.UserRole.Employee or Domain.Enums.UserRole.Admin;
                        var hasScope = requiredScopes.Length == 0 || requiredScopes.All(context.User.HasScope);

                        return hasRole && hasScope;
                    }));
        });

        return services;
    }
}
