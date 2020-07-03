using Android.Content;
using Acciona.Presentation.Navigation;
using Droid.Navigation;
using Acciona.Droid.UI.Features.AlertDetail;

namespace Acciona.Droid.Navigation
{
    public class AlertsNavigator : BaseNavigator, IAlertsNavigator
    {
        public void GoBack()
        {
            activity.Finish();
        }

        public void GoToAlertDetail()
        {
            Intent intent = new Intent(activity, typeof(AlertDetailActivity));
            activity.StartActivity(intent);
        }
    }
}