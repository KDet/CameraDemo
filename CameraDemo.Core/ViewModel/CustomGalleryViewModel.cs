using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CameraDemo.Core.Extensions;
using CameraDemo.Core.Plugins;
using CameraDemo.Core.Services;
using GalaSoft.MvvmLight.Command;
using PCLStorage;

namespace CameraDemo.Core.ViewModel
{
    public class CustomGalleryViewModel : BaseViewModel
    {
        private readonly IDetailService _detailService;
        private readonly INotificationService _notificationService;
        private readonly IUserFolders _userFolders;
        private ObservableCollection<string> _images;
        private RelayCommand<string> _showImageCommand;
        private RelayCommand _getItemsCommand;
        private bool _isRefreshing;

        public ObservableCollection<string> Images
        {
            get { return _images; }
            set { Set(() => Images, ref _images, value); }
        }
        public bool IsRefreshing
        {
            get { return _isRefreshing; }
            set { Set(() => IsRefreshing, ref _isRefreshing, value); }
        }

        public RelayCommand<string> ShowImageCommand =>
            _showImageCommand ?? (_showImageCommand = new RelayCommand<string>(async s => await _detailService.ShowImage(s), s => !IsBusy));

        public RelayCommand GetItemsCommand =>
            _getItemsCommand ?? (_getItemsCommand = new RelayCommand(async () => await GetItems(), () => !IsBusy));

        
        private async Task GetItems()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            IsRefreshing = true;
            GetItemsCommand.RaiseCanExecuteChanged();
            ShowImageCommand.RaiseCanExecuteChanged();
            try
            {
                IFolder rootFolder = await FileSystem.Current.GetFolderFromPathAsync(_userFolders.Pictures);
                IList<IFile> files = await rootFolder.GetFilesAsync();
                Images = new ObservableCollection<string>(files.Select(file => file.Path));
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
                GetItemsCommand.RaiseCanExecuteChanged();
                ShowImageCommand.RaiseCanExecuteChanged();
                IsRefreshing = false;
            }
        }

        public CustomGalleryViewModel(IDetailService detailService, INotificationService notificationService, IUserFolders userFolders)
        {
            _detailService = detailService;
            _notificationService = notificationService;
            _userFolders = userFolders;
            Title = MenuViewModel.CustomGalleryItemTitle;
        }

        public override void OnNavigatedTo()
        {
            base.OnNavigatedTo();
            if(GetItemsCommand.CanExecute(null))
                GetItemsCommand.Execute(null);
        }

    }
}
