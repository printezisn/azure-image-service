using System.IO;
using System.Threading.Tasks;

namespace ImageService.Core
{
    public interface IFileRepository
    {
        string BaseImageUrl { get; }
        Task UploadFile(string filename, Stream stream);
        Task<Stream> DownloadFile(string filename);
    }
}