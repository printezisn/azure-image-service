using System.Threading.Tasks;
using ImageService.Core;
using ImageService.Core.Models;
using ImageService.FunctionApp;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace ImageService.UnitTests
{
    public class TransformImageTests
    {
        private readonly Mock<IQueueRepository> _queueRepository;
        private readonly Mock<ILogger<TransformImageTests>> _logger;
        private readonly TransformImage _function;

        public TransformImageTests()
        {
            _queueRepository = new Mock<IQueueRepository>();
            _logger = new Mock<ILogger<TransformImageTests>>();
            _function = new TransformImage(_queueRepository.Object);
        }

        [Fact]
        public async Task TestTransformImage()
        {
            await _function.Run(null, "test.png", _logger.Object);

            _queueRepository.Verify(v => v.SendMessage(It.Is<TransformImageModel>(i => i.Image == "test.png" && i.Size == 256)));
            _queueRepository.Verify(v => v.SendMessage(It.Is<TransformImageModel>(i => i.Image == "test.png" && i.Size == 128)));
            _queueRepository.Verify(v => v.SendMessage(It.Is<TransformImageModel>(i => i.Image == "test.png" && i.Size == 64)));
        }
    }
}