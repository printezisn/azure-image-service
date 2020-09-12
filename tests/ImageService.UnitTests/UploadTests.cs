using System.IO;
using System.Threading.Tasks;
using ImageService.Core;
using ImageService.FunctionApp;
using ImageService.FunctionApp.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace ImageService.UnitTests
{
    public class UploadTests
    {
        private readonly Mock<IRequestHelper> _requestHelper;
        private readonly Mock<IFileRepository> _fileRepository;
        private readonly Mock<IFormFile> _file;
        private readonly Mock<Stream> _stream;
        private readonly Mock<HttpRequest> _request;
        private readonly Mock<ILogger<Upload>> _logger;
        private readonly Upload _function;

        public UploadTests()
        {
            _requestHelper = new Mock<IRequestHelper>();
            _fileRepository = new Mock<IFileRepository>();
            _file = new Mock<IFormFile>();
            _stream = new Mock<Stream>();
            _request = new Mock<HttpRequest>();
            _logger = new Mock<ILogger<Upload>>();
            _function = new Upload(_requestHelper.Object, _fileRepository.Object);
        }

        [Fact]
        public async Task TestUploadWithNoFile()
        {
            var result = (await _function.Run(_request.Object, _logger.Object)) as BadRequestObjectResult;

            Assert.NotNull(result);
        }

        [Fact]
        public async Task TestUploadWithInvalidImage()
        {
            _requestHelper.Setup(s => s.GetFile(_request.Object)).Returns(_file.Object);
            _file.Setup(s => s.ContentType).Returns("text/plain");

            var result = (await _function.Run(_request.Object, _logger.Object)) as BadRequestObjectResult;

            Assert.NotNull(result);
        }

        [Fact]
        public async Task TestUploadWithSuccess()
        {
            _requestHelper.Setup(s => s.GetFile(_request.Object)).Returns(_file.Object);
            _file.Setup(s => s.ContentType).Returns("image/png");
            _file.Setup(s => s.FileName).Returns("test.png");
            _file.Setup(s => s.OpenReadStream()).Returns(_stream.Object);

            _fileRepository.Setup(s => s.BaseImageUrl).Returns("http://localhost/");

            var result = (await _function.Run(_request.Object, _logger.Object)) as OkObjectResult;

            _fileRepository.Verify(s => s.UploadFile(It.IsAny<string>(), _stream.Object));

            Assert.NotNull(result);
        }
    }
}