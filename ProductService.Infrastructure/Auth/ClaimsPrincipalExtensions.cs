using System.Security.Claims;

namespace ProductService.Infrastructure.Auth
{
    public static class ClaimsPrincipalExtensions
    {
        public static Guid GetUserId(this ClaimsPrincipal user)
        {
            var sub = user.FindFirst(ClaimTypes.NameIdentifier)
                      ?? user.FindFirst("sub");
            return Guid.TryParse(sub.Value, out var id) ? id : throw new UnauthorizedAccessException("Invalid token.");
        }
    }
}
