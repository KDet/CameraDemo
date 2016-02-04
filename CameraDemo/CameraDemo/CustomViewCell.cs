using Xamarin.Forms;

namespace CameraDemo.XamFormsUI
{
    public class CustomViewCell : ViewCell
    {
        readonly Image _image;
        string _cachedImageFile;

        public CustomViewCell()
        {
            _image = new Image();
            View = _image;
        }

        void LoadImage()
        {
            if (_image.Source == null && !string.IsNullOrEmpty(_cachedImageFile))
                _image.Source = ImageSource.FromFile(_cachedImageFile);
        }

        protected override void OnParentSet()
        {
            base.OnParentSet();

            if (_image.Source != null)
                _cachedImageFile = ((FileImageSource) _image.Source).File;

            Page page = null;
            Element currentElement = this;

            while (page == null)
            {
                page = currentElement as Page;
                if (page == null)
                {
                    currentElement = currentElement.Parent;
                }
            }

            page.Appearing += (sender, e) => {
                LoadImage();
            };

            page.Disappearing += (sender, e) => {
                _image.Source = null;
            };
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            LoadImage();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            _image.Source = null;
        }
    }
}
