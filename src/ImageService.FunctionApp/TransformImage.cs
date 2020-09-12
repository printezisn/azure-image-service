using System;
using System.IO;
using System.Threading.Tasks;
using ImageService.Core;
using ImageService.Core.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace ImageService.FunctionApp
{
    public class TransformImage
    {
        private readonly IQueueRepository _queueRepository;

        public TransformImage(IQueueRepository queueRepository)
        {
            _queueRepository = queueRepository;
        }

        [FunctionName("TransformImage")]
        public async Task Run([BlobTrigger("%MainImageContainer%/{name}", Connection = "AzureWebJobsStorage")] Stream myBlob, string name, ILogger log)
        {
            log.LogInformation($"Function triggered to transform image {name}");

            await _queueRepository.SendMessage(new TransformImageModel() { Image = name, Size = 256 });
            await _queueRepository.SendMessage(new TransformImageModel() { Image = name, Size = 128 });
            await _queueRepository.SendMessage(new TransformImageModel() { Image = name, Size = 64 });
        }
    }
}
