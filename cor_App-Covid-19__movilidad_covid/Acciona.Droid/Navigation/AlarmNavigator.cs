using Android.Content;
using Acciona.Presentation.Navigation;
using Droid.Navigation;

namespace Acciona.Droid.Navigation
{
    public class AlarmNavigator : BaseNavigator, IAlarmNavigator
    {
        public void GoBack()
        {
            activity.Finish();
        }
    }
}