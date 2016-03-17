using System;
using System.Threading.Tasks;
using CameraDemo.Core.Extensions;
using CameraDemo.Core.Services;
using GalaSoft.MvvmLight.Command;

namespace CameraDemo.Core.ViewModel
{
    public class GalleryViewModel : BaseViewModel
    {
        private const string GalleryShowErrorMessage = @"Can't open gallery";

        private readonly IMediaService _mediaService;
        private readonly INotificationService _notificationService;
        private readonly IDetailService _detailService;

        private RelayCommand _showGalleryCommand;

        public RelayCommand ShowGalleryCommand =>
            _showGalleryCommand ?? (_showGalleryCommand = new RelayCommand(ShowGallery));

        private async void ShowGallery()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;
                ShowGalleryCommand.RaiseCanExecuteChanged();
                var path = await _mediaService.PickPhotoPathAsync();
                await _detailService.ShowImage(path);
            }
            catch (Exception ex)
            {
#if DEBUG
                _notificationService.DisplayAlert($"{GalleryShowErrorMessage}{Environment.NewLine}{ex.Message}");
#else
                _notificationService.DisplayAlert(GalleryShowErrorMessage);
#endif

            }
            finally
            {
                IsBusy = false;
                ShowGalleryCommand.RaiseCanExecuteChanged();
            }
        }

		/// <summary>
		/// CHANGED
		/// </summary>
		/// <param name="mediaService"></param>
		/// <param name="notificationService"></param>
		/// <param name="detailService"></param>
        public GalleryViewModel(IMediaService mediaService,
                                INotificationService notificationService,
                                IDetailService detailService)
        {
            _mediaService = mediaService;
            _notificationService = notificationService;
            _detailService = detailService;
            Title = MenuViewModel.GalleryItemTitle;
        }

        public override void OnNavigatedTo()
        {
            base.OnNavigatedTo();
            if (ShowGalleryCommand.CanExecute(null))
                ShowGalleryCommand.Execute(null);
        }
    }
}
