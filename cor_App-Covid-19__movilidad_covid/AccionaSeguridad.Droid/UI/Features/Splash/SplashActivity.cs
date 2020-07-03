using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Droid.UI;

namespace AccionaSeguridad.Droid.UI.Features.Splash
{
    [Activity(MainLauncher = true, Icon = "@mipmap/ic_launcher", Theme = "@style/AppTheme", NoHistory = true, ScreenOrientation = ScreenOrientation.Portrait, WindowSoftInputMode = SoftInput.AdjustPan)]
    public class SplashActivity : BasicActivity
    {
        protected override Android.Support.V4.App.Fragment CreateFragment()
        {
            return SplashFragment.NewInstance();
        }
    }
}