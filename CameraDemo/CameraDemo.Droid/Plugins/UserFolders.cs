using Android.OS;
using CameraDemo.Core.Plugins;
using CameraDemo.Droid.Plugins;

[assembly: Xamarin.Forms.Dependency(typeof(UserFolders))]

namespace CameraDemo.Droid.Plugins
{
    public class UserFolders : IUserFolders
    {
        public string Pictures
        {
            get { return Environment.GetExternalStoragePublicDirectory(Environment.DirectoryPictures)?.AbsolutePath; }
        }
    }
}