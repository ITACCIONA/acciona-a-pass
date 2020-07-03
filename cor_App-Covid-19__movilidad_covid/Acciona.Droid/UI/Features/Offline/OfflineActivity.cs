using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Droid.UI;

namespace Acciona.Droid.UI.Features.Offline
{
    [Activity(Label = "OfflineActivity", ScreenOrientation = ScreenOrientation.Portrait, WindowSoftInputMode = SoftInput.AdjustPan)]
    public class OfflineActivity : BasicActivity
    {
        protected override Android.Support.V4.App.Fragment CreateFragment()
        {
            return OfflineFragment.NewInstance();
        }
    }
}