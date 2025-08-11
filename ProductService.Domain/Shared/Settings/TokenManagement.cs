using Newtonsoft.Json;

namespace ProductService.Domain.Shared.Settings
{
    public class TokenManagement
    {
        [JsonProperty("secret")]
        public string Secret { get; set; }

        [JsonProperty("issuer")]
        public string Issuer { get; set; }

        [JsonProperty("audience")]
        public string Audience { get; set; }

        [JsonProperty("accessExpiration")]
        public int AccessExpiration { get; set; }

        [JsonProperty("refreshExpiration")]
        public int RefreshExpiration { get; set; }
        public IEnumerable<AppUser> AppUsers { get; set; }
        public List<string> AppAccessGroups { get; set; }
    }
}
