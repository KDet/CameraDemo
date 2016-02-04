using System;
using System.IO;

namespace CameraDemo.Core.ViewModel
{
    public class MediaViewModel : BaseViewModel
    {
        private string _selectedImage;

        public string SelectedImage
        {
            get { return _selectedImage; }
            set
            {
                if (Set(() => SelectedImage, ref _selectedImage, value))
                    Title = string.IsNullOrWhiteSpace(value) ? string.Empty : Path.GetFileName(value);
            }
        }
    }
}
