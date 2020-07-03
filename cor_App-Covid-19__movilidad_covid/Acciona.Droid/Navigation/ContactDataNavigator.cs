using Android.Content;
using Acciona.Presentation.Navigation;
using Acciona.Droid.UI.Features.Login;
using Droid.Navigation;

namespace Acciona.Droid.Navigation
{
    public class ContactDataNavigator : BaseNavigator, IContactDataNavigator
    {
        public void GoBack()
        {
            activity.Finish();
        }
    }
}