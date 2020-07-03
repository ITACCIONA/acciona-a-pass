using Android.Content;
using Droid.Navigation;
using Acciona.Presentation.Navigation;
using System;
using Acciona.Droid.UI.Features.Login;
using Acciona.Droid.UI.Features.Main;
using Acciona.Droid.UI.Features.MedicalTest;
using Acciona.Droid.UI.Features.Offline;

namespace Acciona.Droid.Navigation
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

        public void GoToMedicalTest()
        {
            Intent intent = new Intent(activity, typeof(MedicalTestActivity));
            activity.StartActivity(intent);
            activity.Finish();
        }

        public void GoToOffline()
        {
            Intent intent = new Intent(activity, typeof(OfflineActivity));
            activity.StartActivity(intent);
            activity.Finish();
        }
    }
}