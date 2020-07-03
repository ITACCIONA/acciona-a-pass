using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using Droid.UI;


namespace Acciona.Droid.UI.Features.Registered
{
    [Activity(Label = "RegisteredActivity", ScreenOrientation = ScreenOrientation.Portrait, WindowSoftInputMode = SoftInput.AdjustPan)]
    public class RegisteredActivity : BasicActivity
    {
        protected override Android.Support.V4.App.Fragment CreateFragment()
        {
            return RegisteredFragment.NewInstance();
        }
    }
}