using System.Security.Claims;

namespace API.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetUsername(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.Name)?.Value;
        }
        public static string GetEmail(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.Email)?.Value;
        }
    }
}