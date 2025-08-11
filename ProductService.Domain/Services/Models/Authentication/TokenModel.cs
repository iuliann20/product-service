using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace ProductService.Domain.Services.Models.Authentication
{
    public class TokenModel
    {
        [Required]
        [JsonProperty("username")]
        public string Username { get; set; }

        [Required]
        [JsonProperty("password")]
        public string Password { get; set; }
    }
}
