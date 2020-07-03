using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Droid.UI;

namespace AccionaSeguridad.Droid.UI.Features.Manual
{
    [Activity(Label = "ManualActivity", ScreenOrientation = ScreenOrientation.Portrait, WindowSoftInputMode = SoftInput.AdjustPan)]
    public class ManualActivity : BasicActivity
    {
        protected override Android.Support.V4.App.Fragment CreateFragment()
        {
            return ManualFragment.NewInstance();
        }
    }
}