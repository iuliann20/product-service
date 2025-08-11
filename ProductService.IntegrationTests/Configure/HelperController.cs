using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using ProductService.Contracts.Authentication;
using ProductService.Domain.Services;
using ProductService.Domain.Services.Models.Authentication;
using ProductService.Domain.Shared.Settings;
using System.Net.Http.Headers;
using System.Text;

namespace ProductService.IntegrationTests.Configure
{
    public class HelperController
    {
        private const string URL_TOKEN = "api/v1/Authentication/token";

        public static void GetToken(CustomWebApplicationFactory factory, HttpClient httpClient)
        {

            var aESEncryptionService = factory.Services.GetRequiredService<IAESEncryptionService>();
            var tokenManagement = factory.Services.GetRequiredService<TokenManagement>();

            var tokenRequest = new TokenRequest
            {
                Username = aESEncryptionService.Decrypt(tokenManagement.AppUsers.FirstOrDefault().User),
                Password = aESEncryptionService.Decrypt(tokenManagement.AppUsers.FirstOrDefault().Pass)
            };

            var tokenResponse = httpClient.PostAsync(URL_TOKEN, new StringContent(JsonConvert.SerializeObject(tokenRequest), Encoding.UTF8, "application/json")).Result;
            var tokenStringResponse = tokenResponse.Content.ReadAsStringAsync();
            var responseToken = JsonConvert.DeserializeObject<TokenResponse>(tokenStringResponse.Result);

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", responseToken.Token);
        }
    }
}
