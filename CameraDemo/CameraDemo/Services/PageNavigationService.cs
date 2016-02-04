using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using CameraDemo.Core.Services;
using CameraDemo.Core.ViewModel;
using GalaSoft.MvvmLight.Views;
using Xamarin.Forms;

namespace CameraDemo.XamFormsUI.Services
{
    //Source: https://raw.githubusercontent.com/mallibone/MvvmLightNavigation.XamarinForms/master/MvvmLightNavigation.XamarinForms/MvvmLightNavigation.XamarinForms/Helpers/NavigationService.cs
    public class PageNavigationService : IPageNavigationService
    {
        protected readonly Dictionary<string, Type> PagesByKey = new Dictionary<string, Type>();
        protected NavigationPage Navigation;

        public string CurrentPageKey
        {
            get
            {
                lock (PagesByKey)
                {
                    if (Navigation?.CurrentPage == null)
                        return null;
                    var pageType = Navigation?.CurrentPage?.GetType();
                    return PagesByKey.ContainsValue(pageType)
                        ? PagesByKey.First(p => p.Value == pageType).Key
                        : null;
                }
            }
        }

        protected Page CreatePage(string pageKey, object parameter)
        {
            lock (PagesByKey)
            {
                if (PagesByKey.ContainsKey(pageKey))
                {
                    var type = PagesByKey[pageKey];
                    ConstructorInfo constructor;
                    object[] parameters;
                    if (parameter == null)
                    {
                        constructor = type.GetTypeInfo()
                            .DeclaredConstructors
                            .FirstOrDefault(c => !c.GetParameters().Any());
                        parameters = new object[] { };
                    }
                    else
                    {
                        constructor = type.GetTypeInfo()
                                          .DeclaredConstructors
                                          .FirstOrDefault(
                                            c =>
                                            {
                                                var p = c.GetParameters();
                                                return p.Count() == 1 && p[0].ParameterType == parameter.GetType();
                                            });
                        parameters = new[] { parameter };
                    }
                    if (constructor == null)
                        throw new InvalidOperationException(
                            $"No suitable constructor found for page {pageKey}");
                    var res = constructor.Invoke(parameters) as Page;
                    return res;

                }
                throw new ArgumentException(
                    $"No such page: {pageKey}. Did you forget to call NavigationService.Configure?",
                    nameof(pageKey));
            }
        }

        public void GoBack()
        {
            if (Navigation != null)
                Navigation.PopAsync();
        }
        public void NavigateTo(string pageKey)
        {
            NavigateTo(pageKey, null);
        }
        public void NavigateTo(string pageKey, object parameter)
        {
            NavigateTo(pageKey, parameter, null);
        }
        public async void NavigateTo(string pageKey, object parameter, Action<BaseViewModel> callBack)
        {
            await NavigateToAsync(pageKey, parameter, callBack);
        }
        public async Task NavigateToAsync(string pageKey, object parameter, Action<BaseViewModel> callBack)
        {
            var page = CreatePage(pageKey, parameter);
            if (page != null && Navigation != null)
            {
                
                await Navigation.PushAsync(page);
                var viewModel = page.BindingContext as BaseViewModel;
                viewModel?.OnNavigatedTo();
                if (callBack != null)
                    callBack(viewModel);
            }
        }

        public void Configure(string pageKey, Type pageType)
        {
            lock (PagesByKey)
            {
                if (PagesByKey.ContainsKey(pageKey))
                    PagesByKey[pageKey] = pageType;
                else
                    PagesByKey.Add(pageKey, pageType);
            }
        }
        public void Initialize(NavigationPage navigation)
        {
            Navigation = navigation;
        }
    }
}
