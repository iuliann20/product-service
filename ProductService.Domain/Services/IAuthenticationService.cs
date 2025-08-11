using Microsoft.AspNetCore.Http;
using ProductService.Domain.Services.Models.Authentication;
using ProductService.Domain.Shared;

namespace ProductService.Domain.Services
{
    public interface IAuthenticationService
    {
        Result<TokenResponse> IsAuthenticated(TokenModel request);
        Result<AccesToken?> Login(string username, IHttpContextAccessor httpContextAccessor);
    }
}
