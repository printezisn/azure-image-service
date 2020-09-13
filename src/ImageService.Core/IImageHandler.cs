using System.IO;

namespace ImageService.Core
{
    public interface IImageHandler
    {
        void Resize(Stream inputStream, Stream outputStream, int size);
    }
}