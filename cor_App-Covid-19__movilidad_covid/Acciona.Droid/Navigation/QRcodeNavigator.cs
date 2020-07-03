using Android.Content;
using Acciona.Presentation.Navigation;
using Droid.Navigation;
using Acciona.Droid.UI.Features.Health;

namespace Acciona.Droid.Navigation
{
    public class QRcodeNavigator : BaseNavigator, IQRcodeNavigator
    {
        public void GotToHealth()
        {
            Intent intent = new Intent(activity, typeof(HealthActivity));
            activity.StartActivity(intent);
        }
    }
}