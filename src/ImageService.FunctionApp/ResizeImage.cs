using System;
using System.IO;
using System.Threading.Tasks;
using ImageService.Core;
using ImageService.Core.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ImageService.FunctionApp
{
    public class ResizeImage
    {
        private readonly IFileRepository _fileRepository;
        private readonly IImageHandler _imageHandler;

        public ResizeImage(IFileRepository fileRepository, IImageHandler imageHandler)
        {
            _fileRepository = fileRepository;
            _imageHandler = imageHandler;
        }

        [FunctionName("ResizeImage")]
        public async Task Run([QueueTrigger("%QueueName%", Connection = "AzureWebJobsStorage")] string message, ILogger log)
        {
            var messageModel = JsonConvert.DeserializeObject<TransformImageModel>(message);
            log.LogInformation($"Function triggered to resize image {messageModel.Image} with size {messageModel.Size}");

            var stream = await _fileRepository.DownloadFile(messageModel.Image);
            using var resizedImageStream = new MemoryStream();

            _imageHandler.Resize(stream, resizedImageStream, messageModel.Size);

            await _fileRepository.UploadFile($"{messageModel.Size}/{messageModel.Image}", resizedImageStream);
        }
    }
}
