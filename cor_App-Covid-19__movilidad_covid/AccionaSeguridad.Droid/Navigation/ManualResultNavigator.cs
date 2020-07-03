using Android.Content;
using AccionaSeguridad.Presentation.Navigation;
using Droid.Navigation;
using AccionaSeguridad.Droid.UI.Features.Result;

namespace AccionaSeguridad.Droid.Navigation
{
    public class ManualResultNavigator : BaseNavigator, IManualResultNavigator
    {
        public void GoResult()
        {
            Intent intent = new Intent(activity, typeof(ResultActivity));
            activity.StartActivity(intent);
            activity.Finish();
        }
    }
}