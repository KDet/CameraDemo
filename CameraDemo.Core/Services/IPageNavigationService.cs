using System;
using System.Threading.Tasks;
using CameraDemo.Core.ViewModel;
using GalaSoft.MvvmLight.Views;

namespace CameraDemo.Core.Services
{
    public interface IPageNavigationService : INavigationService
    {
        void NavigateTo(string pageKey, object parameter, Action<BaseViewModel> callBack);
        Task NavigateToAsync(string pageKey, object parameter, Action<BaseViewModel> callBack);
    }
}
