using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        private readonly IEnumerable<int> _sizes;

        public TransformImage(IQueueRepository queueRepository)
        {
            _queueRepository = queueRepository;
            _sizes = Environment.GetEnvironmentVariable("Sizes")
                .Split(',')
                .Select(s => int.Parse(s));
        }

        [FunctionName("TransformImage")]
        public async Task Run([BlobTrigger("%MainImageContainer%/{name}", Connection = "AzureWebJobsStorage")] Stream myBlob, string name, ILogger log)
        {
            log.LogInformation($"Function triggered to transform image {name}");
            if (name.Contains("/"))
            {
                return;
            }

            foreach (int size in _sizes)
            {
                await _queueRepository.SendMessage(new TransformImageModel() { Image = name, Size = size });
            }
        }
    }
}
