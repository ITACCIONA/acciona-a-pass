using Android.Content;
using AccionaSeguridad.Presentation.Navigation;
using AccionaSeguridad.Droid.UI.Features.Login;
using Droid.Navigation;
using AccionaSeguridad.Droid.UI.Features.Result;
using AccionaSeguridad.Droid.UI.Features.Manual;
using AccionaSeguridad.Droid.UI.Features.Config;
using AccionaSeguridad.Droid.UI.Features.Splash;
using AccionaSeguridad.Droid.UI.Features.WorkingCenter;

namespace AccionaSeguridad.Droid.Navigation
{
    public class MainNavigator : BaseNavigator, IMainNavigator
    {
        public void GoConfig()
        {
            Intent intent = new Intent(activity, typeof(ConfigActivity));
            activity.StartActivity(intent);
        }

        public void GoManual()
        {
            Intent intent = new Intent(activity, typeof(ManualActivity));
            activity.StartActivity(intent);
        }

        public void GoResult()
        {
            Intent intent = new Intent(activity, typeof(ResultActivity));
            activity.StartActivity(intent);
        }

        public void GoSplash()
        {
            Intent intent = new Intent(activity, typeof(SplashActivity));
            activity.StartActivity(intent);
            activity.Finish();
        }

        public void GoToLogin()
        {
            Intent intent = new Intent(activity, typeof(LoginActivity));
            activity.StartActivity(intent);
            activity.Finish();
        }

        public void OpenCenter()
        {
            Intent intent = new Intent(activity, typeof(WorkingCenterActivity));
            activity.StartActivity(intent);
        }
    }
}