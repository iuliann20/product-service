using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace ProductService.Configuration
{
    public static class JwtAuthExtensions
    {
        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration cfg)
        {
            var issuer = cfg["Jwt:Issuer"] ?? throw new InvalidOperationException("Jwt:Issuer missing");
            var audience = cfg["Jwt:Audience"] ?? throw new InvalidOperationException("Jwt:Audience missing");
            var key = cfg["Jwt:SigningKey"] ?? throw new InvalidOperationException("Jwt:SigningKey missing");

            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false; // dev
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = issuer,
                        ValidateAudience = true,
                        ValidAudience = audience,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.FromSeconds(30),

                        // IMPORTANT pt. [Authorize(Roles="Admin")] & userId
                        RoleClaimType = ClaimTypes.Role,
                        NameClaimType = "sub" // noi am pus sub în tokenul emis de UserService
                    };
                });

            services.AddAuthorization(); // poți adăuga politici dacă vrei

            return services;
        }

    }
}