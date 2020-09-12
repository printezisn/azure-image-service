using System.Threading.Tasks;
using ImageService.Core.Models;

namespace ImageService.Core
{
    public interface IQueueRepository
    {
        Task SendMessage(TransformImageModel messageModel);
    }
}