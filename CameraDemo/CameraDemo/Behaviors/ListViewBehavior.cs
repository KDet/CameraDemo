using System;
using Xamarin.Forms;

namespace CameraDemo.XamFormsUI.Behaviors
{
    public class ListViewBehavior : EventToCommandBehavior
    {
        protected override void OnFired(EventArgs args)
        {
            var tappedEventArgs = args as ItemTappedEventArgs;
            if (tappedEventArgs != null)
                CommandParameter = tappedEventArgs.Item;
            else
            {
                var selectedEventArgs = args as SelectedItemChangedEventArgs;
                if (selectedEventArgs != null)
                    CommandParameter = selectedEventArgs.SelectedItem;
            }
            base.OnFired(args);
        }
    }
}
