using System;
using System.IO;
using ImageService.Core;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Processing;

namespace ImageService.Infrastructure
{
    public class ImageHandler : IImageHandler
    {
        public void Resize(Stream inputStream, Stream outputStream, int size)
        {
            using var image = Image.Load(inputStream, out IImageFormat format);
            size = Math.Min(size, image.Width);

            image.Mutate(m => m.Resize(size, 0));

            image.Save(outputStream, format);

            outputStream.Seek(0, SeekOrigin.Begin);
        }
    }
}