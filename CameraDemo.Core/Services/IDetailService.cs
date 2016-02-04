using System;
using CameraDemo.Core.ViewModel;

namespace CameraDemo.Core.Services
{
    public interface IDetailService : IPageNavigationService
    {
        bool IsPresented { get; set; }

        void DetailTo(string pageKey);
        void DetailTo(string pageKey, object parameter);
        void DetailTo(string pageKey, object parameter, Action<BaseViewModel> callBack);
    }
}
