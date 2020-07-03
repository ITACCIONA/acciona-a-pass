
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using Droid.UI;


namespace Acciona.Droid.UI.Features.Health
{
    [Activity(Label = "HealthActivity", ScreenOrientation = ScreenOrientation.Portrait, WindowSoftInputMode = SoftInput.AdjustPan)]
    public class HealthActivity : BasicActivity
    {
        protected override Android.Support.V4.App.Fragment CreateFragment()
        {
            return HealthFragment.NewInstance();
        }
    }
}