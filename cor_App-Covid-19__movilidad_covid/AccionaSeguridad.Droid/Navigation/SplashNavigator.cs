using Android.Content;
using Droid.Navigation;
using AccionaSeguridad.Presentation.Navigation;
using System;
using AccionaSeguridad.Droid.UI.Features.Login;
using AccionaSeguridad.Droid.UI.Features.Main;

namespace AccionaSeguridad.Droid.Navigation
{
    public class SplashNavigator : BaseNavigator, ISplashNavigator
    {
       
        public void GoToLogin()
        {
            Intent intent = new Intent(activity, typeof(LoginActivity));
            activity.StartActivity(intent);
            activity.Finish();
        }

        public void GoToMain()
        {
            Intent intent = new Intent(activity, typeof(MainActivity));
            activity.StartActivity(intent);
            activity.Finish();
        }
    }
}