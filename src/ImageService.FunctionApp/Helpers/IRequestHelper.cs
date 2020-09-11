using System.IO;
using Microsoft.AspNetCore.Http;

namespace ImageService.FunctionApp.Helpers
{
    public interface IRequestHelper
    {
        IFormFile GetFile(HttpRequest request);
    }
}