using Android.Content;
using Acciona.Presentation.Navigation;
using Droid.Navigation;
using Android.Support.V4.App;

namespace Acciona.Droid.Navigation
{
    public class WorkingCenterNavigator : BaseNavigator, IWorkingCenterNavigator
    {
        public void GoBack()
        {
            activity.Finish();
        }        
    }
}