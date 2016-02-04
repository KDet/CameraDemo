using Windows.Storage;
using CameraDemo.Core.Plugins;
using CameraDemo.WinPhone.Plugins;

[assembly: Xamarin.Forms.Dependency(typeof(UserFolders))]

namespace CameraDemo.WinPhone.Plugins
{
    public class UserFolders : IUserFolders
    {
        public string Pictures
        {
            get { return KnownFolders.PicturesLibrary.Path; } 
        }
    }
}
