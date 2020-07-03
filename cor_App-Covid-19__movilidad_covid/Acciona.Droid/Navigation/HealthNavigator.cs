using Android.Content;
using Acciona.Presentation.Navigation;
using Droid.Navigation;

namespace Acciona.Droid.Navigation
{
    public class HealthNavigator : BaseNavigator, IHealthNavigator
    {
        public void GoBack()
        {
            activity.Finish();
        }
    }
}