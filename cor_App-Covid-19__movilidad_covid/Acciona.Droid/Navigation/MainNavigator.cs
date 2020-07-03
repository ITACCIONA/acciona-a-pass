using Android.Content;
using Acciona.Presentation.Navigation;
using Acciona.Droid.UI.Features.Login;
using Droid.Navigation;
using Acciona.Droid.UI.Features.Alarm;
using Acciona.Droid.UI.Features.Alerts;
using Acciona.Droid.UI.Features.Health;

namespace Acciona.Droid.Navigation
{
    public class MainNavigator : BaseNavigator, IMainNavigator
    {
        public void GoToAlarm()
        {
            Intent intent = new Intent(activity, typeof(AlarmActivity));
            activity.StartActivity(intent);
        }

        public void GoToAlerts()
        {
            Intent intent = new Intent(activity, typeof(AlertsActivity));
            activity.StartActivity(intent);
        }

        public void GotToHealth()
        {
            Intent intent = new Intent(activity, typeof(HealthActivity));
            activity.StartActivity(intent);
        }
    }
}