using System.IdentityModel.Tokens.Jwt;

namespace ProductService.Domain.Services.Models.Authentication
{
    public class AccesToken
    {
        public AccesToken(JwtSecurityToken securityToken, string username)
        {
            Token = new JwtSecurityTokenHandler().WriteToken(securityToken);
            TokenType = "Bearer";
            ExpiresTime = securityToken.ValidTo.ToLocalTime();
            Username = username;
        }
        public string Token { get; set; }
        public string TokenType { get; set; }
        public DateTime ExpiresTime { get; set; }
        public string Username { get; set; }

    }
}
