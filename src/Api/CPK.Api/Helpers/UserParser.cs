using System.Security.Claims;

namespace CPK.Api.Helpers
{
    public static class UserParser
    {
        public static string GetId(this ClaimsPrincipal user) => user.FindFirst(ClaimTypes.NameIdentifier).Value;
    }
}