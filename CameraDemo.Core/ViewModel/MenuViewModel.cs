using System.Collections.ObjectModel;
using CameraDemo.Core.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace CameraDemo.Core.ViewModel
{
    public class MenuViewModel : BaseViewModel
    {
        public class MenuItemViewModel : ViewModelBase
        {
            private string _title;
            private string _iconSource;

            public string ID { get; private set; }

            public string Title
            {
                get { return _title; }
                set { Set(() => Title, ref _title, value); }
            }

            public string IconSource
            {
                get { return _iconSource; }
                set { Set(() => IconSource, ref _iconSource, value); }
            }

            public MenuItemViewModel(string id)
            {
                ID = id;
            }

            public MenuItemViewModel(string id, string title, string iconSource) : this(id)
            {
                _title = title;
                _iconSource = iconSource;
            }
        }

        private const string MenuTitle = "Menu";
        public const string CustomGalleryItemTitle = "Custom Gallery";
        public const string GalleryItemTitle = "Gallery";
        public const string CameraItemTitle = "Camera";

        private readonly IDetailService _detailService;
        private RelayCommand<MenuItemViewModel> _detailCommand;

        public ObservableCollection<MenuItemViewModel> MenuItems { get; private set; } = new ObservableCollection<MenuItemViewModel>(new[]
        {
            new MenuItemViewModel(id: nameof(CustomGalleryViewModel), title: CustomGalleryItemTitle, iconSource: null),
            new MenuItemViewModel(id: nameof(GalleryViewModel), title: GalleryItemTitle, iconSource: null),
            new MenuItemViewModel(id: nameof(CameraViewModel), title: CameraItemTitle, iconSource: null)
        });

        public RelayCommand<MenuItemViewModel> DetailCommand =>
            _detailCommand ?? (_detailCommand = new RelayCommand<MenuItemViewModel>(Detail));

        private void Detail(MenuItemViewModel menuItem)
        {
            _detailService.DetailTo(menuItem.ID);
        }

        public MenuViewModel(IDetailService detailService)
        {
            Title = MenuTitle;
            _detailService = detailService;
        }

		public override void OnNavigatedTo()
		{
			base.OnNavigatedTo();
            if(DetailCommand.CanExecute(MenuItems[0]))
                DetailCommand.Execute(MenuItems[0]);
        }
    }
}
