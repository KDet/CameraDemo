using System;
using System.Diagnostics;
using Acr.UserDialogs;
using CameraDemo.Core.Services;

namespace CameraDemo.XamFormsUI.Services
{
    class NotificationService : INotificationService
    {
        public void DisplayAlert(string message, string title = null)
        {
            try
            {
                UserDialogs.Instance.Alert(message, title, "OK");
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
            }
        }
    }
}
