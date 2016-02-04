using System;
using System.IO;
using System.Threading.Tasks;
using CameraDemo.Core.Services;
using Media.Plugin;
using Media.Plugin.Abstractions;
using Splat;

namespace CameraDemo.XamFormsUI.Services
{
    public class MediaService : IMedia, IMediaService
    {
        private readonly IMedia _media = CrossMedia.Current;

        public async Task<string> TakePhotoPathAsync(string directory, string name, float quality)
        {
            using (var res = await TakePhotoAsync(new StoreCameraMediaOptions
            {
                Directory = directory,
                Name = name,
                DefaultCamera = CameraDevice.Front
            }))
            {
                using (var stream = res?.GetStream())
                {
                    var bitmap = await BitmapLoader.Current.Load(stream, null, null);
                    await bitmap.Save(CompressedBitmapFormat.Jpeg, quality, stream);
                }
                return res?.Path;
            }
        }
        public async Task<string> PickPhotoPathAsync()
        {
            using (var res = await PickPhotoAsync())
                return res?.Path;
        }

        public Task<MediaFile> PickPhotoAsync()
        {
           return _media.PickPhotoAsync();
        }
        public Task<MediaFile> TakePhotoAsync(StoreCameraMediaOptions options)
        {
            return _media.TakePhotoAsync(options);
        }
        public Task<MediaFile> PickVideoAsync()
        {
            return _media.PickVideoAsync();
        }
        public Task<MediaFile> TakeVideoAsync(StoreVideoOptions options)
        {
            return _media.TakeVideoAsync(options);
        }

        public bool IsCameraAvailable => _media.IsCameraAvailable;
        public bool IsTakePhotoSupported => _media.IsTakePhotoSupported;
        public bool IsPickPhotoSupported => _media.IsPickPhotoSupported;
        public bool IsTakeVideoSupported => _media.IsTakeVideoSupported;
        public bool IsPickVideoSupported => _media.IsPickVideoSupported;
    }
}
