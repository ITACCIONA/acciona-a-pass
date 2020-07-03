using Android.Content;
using Droid.Navigation;
using AccionaSeguridad.Presentation.Navigation;
using System;
using AccionaSeguridad.Droid.UI.Features.Main;

namespace AccionaSeguridad.Droid.Navigation
{
    public class LoginNavigator : BaseNavigator, ILoginNavigator
    {
        public void GoToMain()
        {
            Intent intent = new Intent(activity, typeof(MainActivity));
            activity.StartActivity(intent);
            activity.Finish();
        }
    }
}