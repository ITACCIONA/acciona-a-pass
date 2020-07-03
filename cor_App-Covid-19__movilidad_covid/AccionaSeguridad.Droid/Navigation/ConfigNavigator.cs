using Android.Content;
using AccionaSeguridad.Presentation.Navigation;
using AccionaSeguridad.Droid.UI.Features.Login;
using Droid.Navigation;
using AccionaSeguridad.Droid.UI.Features.Language;
using AccionaSeguridad.Droid.UI.Features.WorkingCenter;

namespace AccionaSeguridad.Droid.Navigation
{
    public class ConfigNavigator : BaseNavigator, IConfigNavigator
    {
        public void GoBack()
        {
            activity.Finish();
        }

        public void GoLanguage()
        {
            Intent intent = new Intent(activity, typeof(LanguageActivity));
            activity.StartActivity(intent);
        }

        public void GoCenter()
        {
            Intent intent = new Intent(activity, typeof(WorkingCenterActivity));
            activity.StartActivity(intent);
        }
    }
}