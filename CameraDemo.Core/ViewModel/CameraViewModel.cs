using System;
using System.IO;
using System.Threading.Tasks;
using CameraDemo.Core.Extensions;
using CameraDemo.Core.Plugins;
using CameraDemo.Core.Services;
using GalaSoft.MvvmLight.Command;
using PCLStorage;

namespace CameraDemo.Core.ViewModel
{
    public class CameraViewModel : BaseViewModel
    {
        private const string PhotoPrefix = "photo";
        private const string NoCameraAvailableMessage = "No camera avaialble";
        private const string FileLocationMessage = "File Location";

        private readonly IMediaService _mediaService;
        private readonly INotificationService _notificationService;
        private readonly IDetailService _detailService;
        private readonly IUserFolders _userFolders;

        private RelayCommand _capturePhotoCommand;

        public RelayCommand CapturePhotoCommand =>
          _capturePhotoCommand ?? (_capturePhotoCommand = new RelayCommand(async () => await CapturePhoto(), 
                                                                           () => _mediaService.IsCameraAvailable && _mediaService.IsTakePhotoSupported && !IsBusy));

        private async Task CapturePhoto()
        {
            if (IsBusy)
                return;

            try
            {
                if (!_mediaService.IsCameraAvailable || !_mediaService.IsTakePhotoSupported)
                {
                    _notificationService.DisplayAlert(NoCameraAvailableMessage);
                    return;
                }
                IsBusy = true;
                CapturePhotoCommand.RaiseCanExecuteChanged();
                
                var file = await _mediaService.TakePhotoPathAsync(DefaultPhotosDirectory, $"{PhotoPrefix}_{Guid.NewGuid().ToString().Replace("-","").Substring(0,5)}.jpg", 0.5f);

                IFolder rootFolder = await FileSystem.Current.GetFolderFromPathAsync(_userFolders.Pictures);
                var photo = await FileSystem.Current.GetFileFromPathAsync(file);
                var photoPath = PortablePath.Combine(rootFolder.Path, photo.Name);
                await photo.MoveAsync(photoPath, NameCollisionOption.GenerateUniqueName);
                if (!string.IsNullOrWhiteSpace(photoPath))
                {
#if DEBUG
                    _notificationService.DisplayAlert(photoPath, FileLocationMessage);
#endif
                    await _detailService.ShowImage(photoPath);
                }
            }
#if DEBUG
            catch (Exception ex)
            {
                _notificationService.DisplayAlert(ex.Message);
            }
#endif
            finally
            {
                IsBusy = false;
                CapturePhotoCommand.RaiseCanExecuteChanged();
            }
            
        }

        public CameraViewModel(IMediaService mediaService, 
                               INotificationService notificationService, 
                               IDetailService detailService, 
                               IUserFolders userFolders)
        {
            _mediaService = mediaService;
            _notificationService = notificationService;
            _detailService = detailService;
            _userFolders = userFolders;
            Title = MenuViewModel.CameraItemTitle;
        }

        public override void OnNavigatedTo()
        {
            base.OnNavigatedTo();
            if (CapturePhotoCommand.CanExecute(null))
                CapturePhotoCommand.Execute(null);
        }
    }
}
