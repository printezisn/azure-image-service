using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using ImageService.Core;
using ImageService.FunctionApp.Helpers;

namespace ImageService.FunctionApp
{
    public class Upload
    {
        private readonly IRequestHelper _requestHelper;
        private readonly IFileRepository _fileRepository;

        public Upload(IRequestHelper requestHelper, IFileRepository fileRepository)
        {
            _requestHelper = requestHelper;
            _fileRepository = fileRepository;
        }

        [FunctionName("Upload")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Function triggered to upload image.");

            var file = _requestHelper.GetFile(req);

            if (file == null)
            {
                return new BadRequestObjectResult("The image file doesn't exist.");
            }

            if (!file.ContentType.StartsWith("image/"))
            {
                return new BadRequestObjectResult("The file is not an image.");
            }

            string extension = Path.GetExtension(file.FileName);
            string filename = Guid.NewGuid() + extension;
            await _fileRepository.UploadFile(filename, file.OpenReadStream());

            return new OkObjectResult(new { Filename = filename, BaseImageUrl = _fileRepository.BaseImageUrl });
        }
    }
}
