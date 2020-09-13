using System.IO;
using System.Threading.Tasks;
using ImageService.Core;
using ImageService.FunctionApp;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace ImageService.UnitTests
{
    public class ResizeImageTests
    {
        private readonly Mock<IFileRepository> _fileRepository;
        private readonly Mock<IImageHandler> _imageHandler;
        private readonly Mock<ILogger<ResizeImage>> _logger;
        private readonly ResizeImage _function;

        public ResizeImageTests()
        {
            _fileRepository = new Mock<IFileRepository>();
            _imageHandler = new Mock<IImageHandler>();
            _logger = new Mock<ILogger<ResizeImage>>();
            _function = new ResizeImage(_fileRepository.Object, _imageHandler.Object);
        }

        [Fact]
        public async Task TestResizeImage()
        {
            using Stream stream = new MemoryStream();
            string message = "{ \"image\": \"test.png\", \"size\": 64 }";

            _fileRepository.Setup(s => s.DownloadFile("test.png")).Returns(Task.FromResult(stream));

            await _function.Run(message, _logger.Object);

            _imageHandler.Verify(v => v.Resize(stream, It.IsAny<Stream>(), 64));
            _fileRepository.Verify(v => v.UploadFile("64/test.png", It.IsAny<Stream>()));
        }
    }
}