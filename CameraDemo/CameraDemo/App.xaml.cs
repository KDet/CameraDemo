using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CameraDemo.XamFormsUI.Pages;
using CameraDemo.Core;
using CameraDemo.Core.Plugins;
using CameraDemo.Core.Services;
using CameraDemo.Core.ViewModel;
using CameraDemo.XamFormsUI.Services;
using GalaSoft.MvvmLight.Ioc;
using Xamarin.Forms;

namespace CameraDemo.XamFormsUI
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            SimpleIoc.Default.Register<IMediaService, MediaService>();
            SimpleIoc.Default.Register<INotificationService, NotificationService>();
            SimpleIoc.Default.Register<IUserFolders>(() => DependencyService.Get<IUserFolders>());

            var detailService = new DetailService();

            SimpleIoc.Default.Register<IDetailService>(() => detailService);

            detailService.Configure(nameof(CustomGalleryViewModel), typeof(CustomGallery));
            detailService.Configure(nameof(GalleryViewModel), typeof(GalleryPage));
            detailService.Configure(nameof(CameraViewModel), typeof(CameraPage));
            detailService.Configure(nameof(MediaViewModel), typeof(ImagePage));


			var firstPage = new MainPage();
			detailService.Initialize(firstPage);

			ViewModelLocator.Setup();

            MainPage = firstPage;
            
        }
       
    }
}
