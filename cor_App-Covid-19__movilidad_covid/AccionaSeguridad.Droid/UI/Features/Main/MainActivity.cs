
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using Droid.UI;
using System.Collections.Generic;
using System.Linq;

namespace AccionaSeguridad.Droid.UI.Features.Main
{
    [Activity(Label = "MainActivity", ScreenOrientation = ScreenOrientation.Portrait, WindowSoftInputMode = SoftInput.AdjustPan)]
    public class MainActivity : BasicActivity
    {
        protected override Android.Support.V4.App.Fragment CreateFragment()
        {
            return MainFragment.NewInstance();
        }
        
    }
}