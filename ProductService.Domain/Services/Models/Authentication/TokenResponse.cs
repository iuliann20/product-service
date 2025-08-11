namespace ProductService.Domain.Services.Models.Authentication
{
    public class TokenResponse
    {
        public string Token { get; set; }
        public string TokenType { get; set; }
        public DateTime ExpiresTime { get; set; }
        public string Username { get; set; }
    }
}
