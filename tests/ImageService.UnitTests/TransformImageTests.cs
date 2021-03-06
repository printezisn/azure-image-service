using System;
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
            Environment.SetEnvironmentVariable("Sizes", "256,128,64");

            _queueRepository = new Mock<IQueueRepository>();
            _logger = new Mock<ILogger<TransformImageTests>>();
            _function = new TransformImage(_queueRepository.Object);
        }

        [Fact]
        public async Task TestTransformImageWithSubfolder()
        {
            await _function.Run(null, "64/test.png", _logger.Object);

            _queueRepository.Verify(v => v.SendMessage(It.IsAny<TransformImageModel>()), Times.Never());
        }

        [Fact]
        public async Task TestTransformImageWithSuccess()
        {
            await _function.Run(null, "test.png", _logger.Object);

            _queueRepository.Verify(v => v.SendMessage(It.Is<TransformImageModel>(i => i.Image == "test.png" && i.Size == 256)));
            _queueRepository.Verify(v => v.SendMessage(It.Is<TransformImageModel>(i => i.Image == "test.png" && i.Size == 128)));
            _queueRepository.Verify(v => v.SendMessage(It.Is<TransformImageModel>(i => i.Image == "test.png" && i.Size == 64)));
        }
    }
}