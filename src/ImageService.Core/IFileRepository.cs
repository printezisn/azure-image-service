using System.IO;
using System.Threading.Tasks;

namespace ImageService.Core
{
    public interface IFileRepository
    {
        string BaseImageUrl { get; }
        Task<string> UploadFile(string filename, string folder, Stream stream);
        Task<Stream> DownloadFile(string filename);
    }
}