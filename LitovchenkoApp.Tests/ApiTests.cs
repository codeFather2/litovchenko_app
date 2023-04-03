using LitovchenkoApp.Exceptions;
using LitovchenkoApp.Models;
using LitovchenkoApp.Tests.Utils;
using System.Net;
using System.Text.Json;
using Xunit;

namespace LitovchenkoApp.Tests
{
 
    public class IntegrationApiTests : IClassFixture<TestWebApplicationFactory<Program>>
    {
        private readonly TestWebApplicationFactory<Program> factory;

        private const string validPassword = "1s";
        private const string registrationUrl = "api/registration";
        private const string countriesUrl = "api/countries";
        private const string provincesUrl = "api/countries";

        public IntegrationApiTests(TestWebApplicationFactory<Program> factory)
        {
            this.factory = factory;
        }

        [Fact]
        public async Task DbSeedTest()
        {
            var client = factory.CreateClient();
            var response = await client.GetAsync(countriesUrl);
            response.EnsureSuccessStatusCode();
            var deserializedData = JsonSerializer.Deserialize<Country[]>(response.Content.ReadAsStream(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            if (deserializedData != null)
            {
                Assert.Equal(3, deserializedData.Length);
                foreach(var country in deserializedData)
                {
                    response = await client.GetAsync(provincesUrl + $"?countryId={country.Id}");
                    response.EnsureSuccessStatusCode();
                    var provinces = JsonSerializer.Deserialize<Province[]>(response.Content.ReadAsStream(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    Assert.True(provinces!.Length >= 2);
                }
            }
        }

        [Fact]
        public async Task RegistrationTest()
        {
            var client = factory.CreateClient();
            var body = JsonContent.Create(new User { Email = "first@user.com", Password = validPassword, ProvinceId = 1});
            var response = await client.PostAsync(registrationUrl, body);
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task RegistrationDuplicateTest()
        {
            var client = factory.CreateClient();
            var body = JsonContent.Create(new User { Email = "second@user.com", Password = validPassword, ProvinceId = 1 });
            var response = await client.PostAsync(registrationUrl, body);
            response.EnsureSuccessStatusCode();
            response = await client.PostAsync(registrationUrl, body);
            Assert.False(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
        }

        [Fact]
        public async Task WeakPasswordTest()
        {
            var client = factory.CreateClient();
            var body = JsonContent.Create(new User { Email = "third@user.com", Password = "A", ProvinceId=1});
            var response = await client.PostAsync(registrationUrl, body);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            var deserializedData = JsonSerializer.Deserialize<ExceptionResponse>(response.Content.ReadAsStream(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            Assert.NotNull(deserializedData);
            Assert.Equal(nameof(BadInputException), deserializedData.Type);
        }
    }
}