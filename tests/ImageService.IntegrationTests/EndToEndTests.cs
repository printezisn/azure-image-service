using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xunit;

namespace ImageService.IntegrationTests
{
    public class EndToEndTests
    {
        private readonly HttpClient _client;

        public EndToEndTests()
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri(Constants.BASE_URL);
        }

        [Fact]
        public async Task TestEndToEnd()
        {
            var fileContent = await File.ReadAllBytesAsync("Fixtures/test.png");
            var byteArrayContent = new ByteArrayContent(fileContent);
            byteArrayContent.Headers.ContentType = new MediaTypeHeaderValue("image/png");

            var multiPartFormDataContent = new MultipartFormDataContent()
            {
                { byteArrayContent, "\"file\"", "\"test.png\"" }
            };

            var response = await _client.PostAsync("api/upload", multiPartFormDataContent);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var uploadResponse = JsonConvert.DeserializeObject<UploadResponse>(content);

            Console.WriteLine(uploadResponse.BaseImageUrl + uploadResponse.Filename);
            response = await _client.GetAsync(uploadResponse.BaseImageUrl + uploadResponse.Filename);
            response.EnsureSuccessStatusCode();
        }
    }
}