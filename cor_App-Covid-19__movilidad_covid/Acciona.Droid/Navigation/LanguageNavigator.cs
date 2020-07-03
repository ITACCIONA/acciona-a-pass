using Android.Content;
using Acciona.Presentation.Navigation;
using Droid.Navigation;
using Acciona.Droid.UI.Features.Splash;
using Android.Support.V4.App;

namespace Acciona.Droid.Navigation
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