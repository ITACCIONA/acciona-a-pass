using Android.Content;
using Droid.Navigation;
using Acciona.Presentation.Navigation;
using System;
using Acciona.Droid.UI.Features.Main;
using Acciona.Droid.UI.Features.Registered;
using Acciona.Droid.UI.Features.MedicalTest;

namespace Acciona.Droid.Navigation
{
    public class LoginNavigator : BaseNavigator, ILoginNavigator
    {
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

        public void GoToRegistered()
        {
            Intent intent = new Intent(activity, typeof(RegisteredActivity));
            activity.StartActivity(intent);
            activity.Finish();
        }
    }
}