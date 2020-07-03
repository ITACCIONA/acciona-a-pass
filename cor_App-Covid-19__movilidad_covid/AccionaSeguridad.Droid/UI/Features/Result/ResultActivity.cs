using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Droid.UI;

namespace AccionaSeguridad.Droid.UI.Features.Result
{
    [Activity(Label = "ResultActivity", ScreenOrientation = ScreenOrientation.Portrait, WindowSoftInputMode = SoftInput.AdjustPan)]
    public class ResultActivity : BasicActivity
    {
        protected override Android.Support.V4.App.Fragment CreateFragment()
        {
            return ResultFragment.NewInstance();
        }
    }
}