using System;
using System.IO;
using System.Threading.Tasks;

namespace CameraDemo.Core.Services
{
    public interface IMediaService
    {
        bool IsCameraAvailable { get; }
        bool IsTakePhotoSupported { get; }

        Task<string> TakePhotoPathAsync(string directory, string name, float quality);
        Task<string> PickPhotoPathAsync();
    }
}
