using Android.Content;
using AccionaSeguridad.Presentation.Navigation;
using Droid.Navigation;
using AccionaSeguridad.Droid.UI.Features.Result;

namespace AccionaSeguridad.Droid.Navigation
{
    public class ManualNavigator : BaseNavigator, IManualNavigator
    {
        public void GoBack()
        {
            activity.Finish();
        }

        public void GoToResult()
        {
            Intent intent = new Intent(activity, typeof(ResultActivity));
            activity.StartActivity(intent);
            activity.Finish();
        }
    }
}