using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ProductService.Domain.Errors;
using ProductService.Domain.Services;
using ProductService.Domain.Services.Models.Authentication;
using ProductService.Domain.Shared;
using ProductService.Domain.Shared.Settings;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ProductService.Infrastructure.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly TokenManagement _tokenManagement;
        private readonly IAESEncryptionService _encryptionService;

        public AuthenticationService(IOptions<TokenManagement> tokenManagement, IAESEncryptionService encryptionService)
        {
            _tokenManagement = tokenManagement.Value;
            _encryptionService = encryptionService;
        }

        public Result<TokenResponse> IsAuthenticated(TokenModel request)
        {
            if (!IsValidUser(request.Username, request.Password))
            {
                return Result.Failure<TokenResponse>(AuthenticationErrors.AuthenticationInvalidUser);
            }

            var claims = CreateClaims(request);

            if (claims.Count == 0)
            {
                return Result.Failure<TokenResponse>(AuthenticationErrors.AuthenticationInvalidClaim);
            }

            var jwtToken = GenerateToken(claims);

            var accesTokenResponse = new AccesToken(jwtToken, request.Username);

            var tokenResponse = new TokenResponse
            {
                Token = accesTokenResponse.Token,
                ExpiresTime = accesTokenResponse.ExpiresTime,
                TokenType = accesTokenResponse.TokenType,
                Username = accesTokenResponse.Username
            };

            return Result.Create(tokenResponse);
        }

        public Result<AccesToken?> Login(string username, IHttpContextAccessor httpContextAccessor)
        {
            foreach (var group in _tokenManagement.AppAccessGroups)
            {
                if (httpContextAccessor.HttpContext.User.IsInRole(group))
                {
                    var claims = CreateCustomClaims(username);
                    var token = GenerateToken(claims);

                    return Result.Create(new AccesToken(token, username.Split("\\")[1]));
                }
            }
            return Result.Failure<AccesToken?>(AuthenticationErrors.AuthenticationError);
        }

        private List<Claim> CreateClaims(TokenModel request)
        {
            var claims = new List<Claim>();
            var user = _tokenManagement.AppUsers.FirstOrDefault(user => _encryptionService.Decrypt(user.Pass) == request.Password && _encryptionService.Decrypt(user.User) == request.Username);

            if (user != null)
            {
                claims.Add(new Claim(ClaimTypes.Name, request.Username));
                user.Roles.ToList().ForEach(role => claims.Add(new Claim(ClaimTypes.Role, role)));
            }

            return claims;
        }

        private static List<Claim> CreateCustomClaims(string username)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, username),
                new(ClaimTypes.Role, Constants.DEFAULT_USER_ROLE)
            };

            return claims;
        }

        private JwtSecurityToken GenerateToken(IEnumerable<Claim> claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenManagement.Secret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var jwtToken = new JwtSecurityToken(
                _tokenManagement.Issuer,
                _tokenManagement.Audience,
                claims,
                notBefore: DateTime.Now,

                expires: DateTime.Now.AddMinutes(_tokenManagement.AccessExpiration),
                signingCredentials: credentials
            );

            return jwtToken;
        }

        private bool IsValidUser(string username, string password)
        {
            return _tokenManagement.AppUsers.Any(user => _encryptionService.Decrypt(user.Pass) == password && _encryptionService.Decrypt(user.User) == username);
        }
    }
}
