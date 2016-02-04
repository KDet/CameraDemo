using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CameraDemo.Core.Services;
using CameraDemo.Core.ViewModel;
using Xamarin.Forms;

namespace CameraDemo.XamFormsUI.Services
{
    public class DetailService : PageNavigationService, IDetailService
    {
        private MasterDetailPage _masterDetailPage;

        public bool IsPresented
        {
            get { return _masterDetailPage != null && _masterDetailPage.IsPresented; }
            set
            {
                if (_masterDetailPage != null)
                    _masterDetailPage.IsPresented = value;
            }
        }

        public void DetailTo(string pageKey)
        {
            DetailTo(pageKey, null);
        }      
        public void DetailTo(string pageKey, object parameter)
        {
            DetailTo(pageKey, parameter, null);
        }
        public void DetailTo(string pageKey, object parameter, Action<BaseViewModel> callBack)
        {
            var page = CreatePage(pageKey, parameter);
            if (_masterDetailPage != null && page != null)
            {
                Navigation = new NavigationPage(page);
                _masterDetailPage.Detail = Navigation;
                IsPresented = false;
                var viewModel = page.BindingContext as BaseViewModel;
                viewModel?.OnNavigatedTo();
                if (callBack != null)
                    callBack(viewModel);
            }
        }

        public void Initialize(MasterDetailPage masterDetailPage)
        {
            _masterDetailPage = masterDetailPage;
            Initialize(masterDetailPage.Detail as NavigationPage);
        }
    }
}
