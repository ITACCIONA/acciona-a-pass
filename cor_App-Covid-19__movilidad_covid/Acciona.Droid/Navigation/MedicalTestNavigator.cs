using Android.Content;
using Acciona.Presentation.Navigation;
using Acciona.Droid.UI.Features.Login;
using Droid.Navigation;
using Acciona.Droid.UI.Features.Main;
using Android.Support.V4.App;

namespace Acciona.Droid.Navigation
{
    public class MedicalTestNavigator : BaseNavigator, IMedicalTestNavigator
    {        

        public void GoToMain()
        {
            Intent intent = new Intent(activity, typeof(MainActivity));
            ActivityCompat.FinishAffinity(activity);
            activity.StartActivity(intent);            
        }

        public void GoBack()
        {
            activity.Finish();
        }
    }
}