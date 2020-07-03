
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using Droid.UI;


namespace Acciona.Droid.UI.Features.Alerts
{
    [Activity(Label = "AlertsActivity", ScreenOrientation = ScreenOrientation.Portrait, WindowSoftInputMode = SoftInput.AdjustPan)]
    public class AlertsActivity : BasicActivity
    {
        protected override Android.Support.V4.App.Fragment CreateFragment()
        {
            return AlertsFragment.NewInstance();
        }
    }
}