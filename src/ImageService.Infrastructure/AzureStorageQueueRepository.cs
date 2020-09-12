using System;
using System.Text;
using System.Threading.Tasks;
using Azure.Storage.Queues;
using ImageService.Core;
using ImageService.Core.Models;
using Newtonsoft.Json;

namespace ImageService.Infrastructure
{
    public class AzureStorageQueueRepository : IQueueRepository
    {
        private readonly string _connectionString;
        private readonly string _queueName;

        private QueueClient _queueClient;
        private QueueClient QueueClient
        {
            get
            {
                if (_queueClient == null)
                {
                    _queueClient = new QueueClient(_connectionString, _queueName);
                }

                return _queueClient;
            }
        }

        public AzureStorageQueueRepository()
        {
            _connectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
            _queueName = Environment.GetEnvironmentVariable("QueueName");
        }

        public async Task SendMessage(TransformImageModel messageModel)
        {
            await QueueClient.CreateIfNotExistsAsync();

            var message = JsonConvert.SerializeObject(messageModel);
            var encodedMessage = Encoding.UTF8.GetBytes(message);

            await QueueClient.SendMessageAsync(Convert.ToBase64String(encodedMessage));
        }
    }
}