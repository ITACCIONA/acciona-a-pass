
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using Droid.UI;


namespace Acciona.Droid.UI.Features.Alarm
{
    [Activity(Label = "AlarmActivity", ScreenOrientation = ScreenOrientation.Portrait, WindowSoftInputMode = SoftInput.AdjustPan)]
    public class AlarmActivity : BasicActivity
    {
        protected override Android.Support.V4.App.Fragment CreateFragment()
        {
            return AlarmFragment.NewInstance();
        }
    }
}