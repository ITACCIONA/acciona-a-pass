
using Acciona.Domain.Model.Employee;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using Droid.UI;
using Newtonsoft.Json;

namespace Acciona.Droid.UI.Features.MedicalInfoEdit
{
    [Activity(Label = "MedicalInfoEditActivity", ScreenOrientation = ScreenOrientation.Portrait, WindowSoftInputMode = SoftInput.AdjustPan)]
    public class MedicalInfoEditActivity : BasicActivity
    {
        protected override Android.Support.V4.App.Fragment CreateFragment()
        {
            var FichaString = Intent.GetStringExtra("ficha");
            var ficha = JsonConvert.DeserializeObject<Ficha>(FichaString);

            return MedicalInfoEditFragment.NewInstance(ficha);
        }
    }
}