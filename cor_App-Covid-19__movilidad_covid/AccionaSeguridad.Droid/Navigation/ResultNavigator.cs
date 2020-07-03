using Android.Content;
using AccionaSeguridad.Presentation.Navigation;
using AccionaSeguridad.Droid.UI.Features.Login;
using Droid.Navigation;

namespace AccionaSeguridad.Droid.Navigation
{
    public class ResultNavigator : BaseNavigator, IResultNavigator
    {
        public void GoBack()
        {
            activity.Finish();
        }

    }
}