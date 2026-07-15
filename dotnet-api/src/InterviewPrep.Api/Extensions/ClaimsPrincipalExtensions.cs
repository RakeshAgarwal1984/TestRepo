using System.Security.Claims;
using System.Text.Json;
using InterviewPrep.Domain.Enums;

namespace InterviewPrep.Api.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static string GetDisplayName(this ClaimsPrincipal user)
    {
        return user.FindFirstValue(ClaimTypes.Name)
            ?? user.FindFirstValue("name")
            ?? user.FindFirstValue("preferred_username")
            ?? "Signed-in user";
    }

    public static UserRole GetRole(this ClaimsPrincipal user)
    {
        return user.TryGetRole(out var role) ? role : UserRole.Employee;
    }

    public static bool TryGetRole(this ClaimsPrincipal user, out UserRole role)
    {
        foreach (var rawRole in user.GetRoleClaimValues())
        {
            if (Enum.TryParse(rawRole, ignoreCase: true, out role))
            {
                return true;
            }
        }

        role = UserRole.Employee;
        return false;
    }

    public static bool HasScope(this ClaimsPrincipal user, string requiredScope)
    {
        var scopes = user.FindAll("scope")
            .Concat(user.FindAll("scp"))
            .SelectMany(claim => claim.Value.Split(' ', StringSplitOptions.RemoveEmptyEntries));

        return scopes.Any(scope => string.Equals(scope, requiredScope, StringComparison.OrdinalIgnoreCase));
    }

    private static IEnumerable<string> GetRoleClaimValues(this ClaimsPrincipal user)
    {
        foreach (var claimType in new[] { ClaimTypes.Role, "role", "roles" })
        {
            foreach (var claim in user.FindAll(claimType))
            {
                yield return claim.Value;
            }
        }

        foreach (var role in ReadKeycloakRoles(user.FindFirstValue("realm_access")))
        {
            yield return role;
        }

        foreach (var role in ReadKeycloakRoles(user.FindFirstValue("resource_access")))
        {
            yield return role;
        }
    }

    private static IEnumerable<string> ReadKeycloakRoles(string? json)
    {
        if (string.IsNullOrWhiteSpace(json))
        {
            yield break;
        }

        using var document = JsonDocument.Parse(json);
        var root = document.RootElement;

        if (root.TryGetProperty("roles", out var realmRoles))
        {
            foreach (var role in ReadRoleArray(realmRoles))
            {
                yield return role;
            }

            yield break;
        }

        foreach (var client in root.EnumerateObject())
        {
            if (!client.Value.TryGetProperty("roles", out var clientRoles))
            {
                continue;
            }

            foreach (var role in ReadRoleArray(clientRoles))
            {
                yield return role;
            }
        }
    }

    private static IEnumerable<string> ReadRoleArray(JsonElement roles)
    {
        if (roles.ValueKind != JsonValueKind.Array)
        {
            yield break;
        }

        foreach (var role in roles.EnumerateArray())
        {
            if (role.ValueKind == JsonValueKind.String)
            {
                yield return role.GetString() ?? string.Empty;
            }
        }
    }
}
