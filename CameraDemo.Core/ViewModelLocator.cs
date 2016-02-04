using CameraDemo.Core.ViewModel;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;

namespace CameraDemo.Core
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        private static ViewModelLocator _locator;
        public static ViewModelLocator Locator
        {
            get { return _locator ?? (_locator = new ViewModelLocator()); }
        }

        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<MediaViewModel>();
			SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<CameraViewModel>();
            SimpleIoc.Default.Register<GalleryViewModel>();
            SimpleIoc.Default.Register<CustomGalleryViewModel>();
            SimpleIoc.Default.Register<MenuViewModel> ();
        }

        public MainViewModel Main => ServiceLocator.Current.GetInstance<MainViewModel>();
        public MenuViewModel Menu => ServiceLocator.Current.GetInstance<MenuViewModel>();
        public MediaViewModel Media => ServiceLocator.Current.GetInstance<MediaViewModel>();
        public CameraViewModel Camera => ServiceLocator.Current.GetInstance<CameraViewModel>();
        public GalleryViewModel Gallery => ServiceLocator.Current.GetInstance<GalleryViewModel>();
        public CustomGalleryViewModel CustomGallery => ServiceLocator.Current.GetInstance<CustomGalleryViewModel>();

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
		public static void Setup()
		{
			Locator.Main.OnNavigatedTo ();
			Locator.Menu.OnNavigatedTo ();
		}
    }
}