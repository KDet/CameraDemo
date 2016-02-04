namespace CameraDemo.Core.Services
{
    public interface INotificationService
    {
        void DisplayAlert(string message, string title = null);
    }
}