using Android.Content;
using Droid.Navigation;
using Acciona.Presentation.Navigation;
using System;
using Acciona.Droid.UI.Features.Login;
using Acciona.Droid.UI.Features.Main;
using Acciona.Droid.UI.Features.MedicalTest;

namespace Acciona.Droid.Navigation
{
    public class OfflineNavigator : BaseNavigator, IOfflineNavigator
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