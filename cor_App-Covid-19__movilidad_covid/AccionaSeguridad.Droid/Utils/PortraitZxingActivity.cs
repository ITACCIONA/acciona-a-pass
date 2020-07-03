using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ZXing.Mobile;

namespace AccionaSeguridad.Droid.Utils
{
    [Activity(Label = "PortraitZxingActivity", ScreenOrientation = ScreenOrientation.Portrait)]
    public class PortraitZxingActivity : ZxingActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
        }
    }
}