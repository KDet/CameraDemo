using System;
using System.Threading.Tasks;
using CameraDemo.Core.Services;
using CameraDemo.Core.ViewModel;

namespace CameraDemo.Core.Extensions
{
    public static class ViewModelExtensions
    {
        public static async Task ShowImage(this IDetailService detailService, string image)
        {
            ViewModelLocator.Locator.Media.SelectedImage = null;
            await detailService.NavigateToAsync(nameof(MediaViewModel), null,
                model =>
                {
                    var mediaViewModel = model as MediaViewModel;
                    if (mediaViewModel != null)
                        mediaViewModel.SelectedImage = image;
                });
        }
    }
}