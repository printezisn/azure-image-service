using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Xunit;

namespace ImageService.IntegrationTests
{
    public class UploadTests
    {
        private HttpClient _client;

        public UploadTests()
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri(Constants.BASE_URL);
        }

        [Fact]
        public async Task TestUpload()
        {
            var fileContent = await File.ReadAllBytesAsync("Fixtures/test.png");
            var byteArrayContent = new ByteArrayContent(fileContent);
            byteArrayContent.Headers.ContentType = new MediaTypeHeaderValue("image/png");

            var multiPartFormDataContent = new MultipartFormDataContent()
            {
                {byteArrayContent, "\"file\"", "\"test.png\""}
            };

            var response = await _client.PostAsync("api/upload", multiPartFormDataContent);
            response.EnsureSuccessStatusCode();
        }
    }
}