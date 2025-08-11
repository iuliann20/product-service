using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace ProductService.Helpers
{
    public class PoliciesHandler
    {
        public static void SetPolicies(AuthorizationOptions options)
        {
            options.AddPolicy("bt-share-holders-web", policy =>
            {
                policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
                policy.RequireAuthenticatedUser();
                policy.RequireRole("bt-share-holders-web");
            });
        }
    }
}
