using Android.Content;
using Acciona.Presentation.Navigation;
using Droid.Navigation;

namespace Acciona.Droid.Navigation
{
    public class WebNavigator : BaseNavigator, IWebNavigator
    {
        public void GoBack()
        {
            activity.Finish();
        }
    }
}