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

        public async Task<string> UploadFile(string filename, string folder, Stream stream)
        {
            var containerClient = new BlobContainerClient(_connectionString, _mainImageContainer);
            await containerClient.CreateIfNotExistsAsync(publicAccessType: PublicAccessType.Blob);

            var extension = Path.GetExtension(filename);
            filename = Guid.NewGuid() + extension;
            if (folder != null)
            {
                filename = $"{folder}/{filename}";
            }

            var blobClient = containerClient.GetBlobClient(filename);
            await blobClient.UploadAsync(stream);

            return filename;
        }

        public Task<Stream> DownloadFile(string filename)
        {
            var containerClient = new BlobContainerClient(_connectionString, _mainImageContainer);
            var blobClient = containerClient.GetBlobClient(filename);

            return blobClient.OpenReadAsync(new BlobOpenReadOptions(false));
        }
    }
}