using System;
using CameraDemo.Core.Plugins;
using CameraDemo.iOS.Plugins;

[assembly: Xamarin.Forms.Dependency(typeof(UserFolders))]

namespace CameraDemo.iOS.Plugins
{
    public class UserFolders : IUserFolders
    {
        public string Pictures
        {
            get
            {
                return Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            }
        }
    }
}
