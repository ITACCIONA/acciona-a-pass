using Android.Content;
using AccionaSeguridad.Presentation.Navigation;
using Droid.Navigation;
using Android.Support.V4.App;

namespace AccionaSeguridad.Droid.Navigation
{
    public class WorkingCenterNavigator : BaseNavigator, IWorkingCenterNavigator
    {
        public void GoBack()
        {
            activity.Finish();
        }        
    }
}