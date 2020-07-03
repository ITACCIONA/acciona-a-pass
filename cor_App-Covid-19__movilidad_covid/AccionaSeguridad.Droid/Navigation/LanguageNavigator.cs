using Android.Content;
using AccionaSeguridad.Presentation.Navigation;
using Droid.Navigation;
using Android.Support.V4.App;
using AccionaSeguridad.Droid.UI.Features.Splash;

namespace AccionaSeguridad.Droid.Navigation
{
    public class LanguageNavigator : BaseNavigator, ILanguageNavigator
    {
        public void GoBack()
        {
            activity.Finish();
        }

        public void RestarApp()
        {
            Intent intent = new Intent(activity, typeof(SplashActivity));
            ActivityCompat.FinishAffinity(activity);
            activity.StartActivity(intent);
        }
    }
}