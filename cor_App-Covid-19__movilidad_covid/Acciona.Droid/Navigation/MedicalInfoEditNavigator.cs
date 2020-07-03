using Android.Content;
using Acciona.Presentation.Navigation;
using Droid.Navigation;
using Acciona.Droid.UI.Features.Main;
using Android.Support.V4.App;

namespace Acciona.Droid.Navigation
{
    public class MedicalInfoEditNavigator : BaseNavigator, IMedicalInfoEditNavigator
    {
        public void GoBack()
        {
            activity.Finish();
        }

        public void GoToMain()
        {
            Intent intent = new Intent(activity, typeof(MainActivity));
            ActivityCompat.FinishAffinity(activity);
            activity.StartActivity(intent);
        }
    }
}