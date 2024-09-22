using System.Security.Claims;

namespace TabibApp.Infrastructure.Identity;

public static class ClaimsPrincipalExtensions
{
    public static string GetUserId(this ClaimsPrincipal @this)
    {
        return @this.FindFirst(ClaimTypes.NameIdentifier).Value;
    }
}