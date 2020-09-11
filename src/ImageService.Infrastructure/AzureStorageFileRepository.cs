using System;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using ImageService.Core;

namespace ImageService.Infrastructure
{
    public class AzureStorageFileRepository : IFileRepository
    {
        private readonly string _connectionString;
        private readonly string _mainImageContainer;

        public string BaseImageUrl => Environment.GetEnvironmentVariable("BaseImageUrl") + _mainImageContainer + "/";

        public AzureStorageFileRepository()
        {
            _connectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
            _mainImageContainer = Environment.GetEnvironmentVariable("MainImageContainer");
        }

        public async Task<string> UploadFile(string filename, Stream stream)
        {
            var containerClient = new BlobContainerClient(_connectionString, _mainImageContainer);
            await containerClient.CreateIfNotExistsAsync(publicAccessType: PublicAccessType.Blob);

            var extension = Path.GetExtension(filename);
            filename = Guid.NewGuid() + extension;

            var blobClient = containerClient.GetBlobClient(filename);
            await blobClient.UploadAsync(stream);

            return filename;
        }
    }
}