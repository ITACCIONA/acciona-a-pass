using Android.Content;
using Acciona.Presentation.Navigation;
using Acciona.Droid.UI.Features.Login;
using Droid.Navigation;
using Acciona.Droid.UI.Features.MedicalTest;

namespace Acciona.Droid.Navigation
{
    public class RegisteredNavigator : BaseNavigator, IRegisteredNavigator
    {
        public void GoToMedicalTest()
        {
            Intent intent = new Intent(activity, typeof(MedicalTestActivity));
            activity.StartActivity(intent);            
        }
    }
}