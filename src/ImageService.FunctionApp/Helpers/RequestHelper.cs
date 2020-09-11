using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace ImageService.FunctionApp.Helpers
{
    public class RequestHelper : IRequestHelper
    {
        public IFormFile GetFile(HttpRequest request)
        {
            return request.Form.Files.FirstOrDefault();
        }
    }
}