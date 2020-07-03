
using Acciona.Domain.Model.Employee;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using Droid.UI;
using Newtonsoft.Json;

namespace AccionaSeguridad.Droid.UI.Features.WorkingCenter
{
    [Activity(Label = "WorkingCenterActivity", ScreenOrientation = ScreenOrientation.Portrait, WindowSoftInputMode = SoftInput.AdjustPan)]
    public class WorkingCenterActivity : BasicActivity
    {
        protected override Android.Support.V4.App.Fragment CreateFragment()
        {           
            return WorkingCenterFragment.NewInstance();
        }
    }
}