
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using Droid.UI;


namespace Acciona.Droid.UI.Features.MedicalTest
{
    [Activity(Label = "MedicalTestActivity", ScreenOrientation = ScreenOrientation.Portrait, WindowSoftInputMode = SoftInput.AdjustPan)]
    public class MedicalTestActivity : BasicActivity
    {
        protected override Android.Support.V4.App.Fragment CreateFragment()
        {
            var canBack = Intent.GetBooleanExtra("canBack", false);
            return MedicalTestFragment.NewInstance(canBack);
        }
    }
}