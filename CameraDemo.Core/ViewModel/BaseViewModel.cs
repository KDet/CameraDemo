using GalaSoft.MvvmLight;

namespace CameraDemo.Core.ViewModel
{
    public class BaseViewModel : ViewModelBase
    {
        protected const string DefaultPhotosDirectory = "CameraDemo";

        private string _title;
        private bool _isBusy;

        public string Title
        {
            get { return _title; }
            set { Set(() => Title, ref _title, value); }
        }
        public bool IsBusy
        {
            get { return _isBusy; }
            set { Set(() => IsBusy, ref _isBusy, value); }
        }

        public virtual void OnNavigatedTo() { }
    }
}
