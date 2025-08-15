using FluentAssertions;
using Newtonsoft.Json;
using ProductService.IntegrationTests.Configure;
using ProductService.IntegrationTests.MockData;
using System.Text;
using Xunit;
namespace ProductService.IntegrationTests.TestController
{
    public class TestControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient httpClient;
        private readonly CustomWebApplicationFactory _factory;
        private const string URL_GET_TEST = "api/v1/Test/getTest";

        public TestControllerIntegrationTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
            httpClient = _factory.CreateClient();
            HelperController.GetToken(factory, httpClient);
        }

        [Fact]
        public async Task GetTest()
        {
            var id = await TestMockData.InsertDataWithDbContext(_factory);

            var testRequest = new List<int>();

            var testRequestData = new StringContent(JsonConvert.SerializeObject(testRequest), Encoding.UTF8, "application/json");

            using var httpResponse = await httpClient.PostAsync(URL_GET_TEST, testRequestData);
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();

            List<int>? response = JsonConvert.DeserializeObject<List<int>>(stringResponse);

            response.Count.Should().NotBe(0);
        }
    }
}
