using ProductService.Domain.Shared;

namespace ProductService.Domain.Errors
{
    public static class AuthenticationErrors
    {
        public static readonly Error AuthenticationInvalidUser = new(
            "Authentication.InvalidUser",
            "Invalid user or password");

        public static readonly Error AuthenticationInvalidClaim = new(
            "Authentication.InvalidClaim",
            "Invalid claim");

        public static readonly Error AuthenticationError = new(
           "Authentication.CanotLogin",
           "Could not be authenticated");
    }
}
