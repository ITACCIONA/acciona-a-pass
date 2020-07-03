using Android.Content;
using Acciona.Presentation.Navigation;
using Acciona.Droid.UI.Features.Login;
using Acciona.Droid.UI.Features.MedicalTest;
using Droid.Navigation;
using Acciona.Domain.Model.Employee;
using Acciona.Droid.UI.Features.MedicalInfoEdit;
using Newtonsoft.Json;

namespace Acciona.Droid.Navigation
{
    public class MedicalInfoNavigator : BaseNavigator, IMedicalInfoNavigator
    {
        public void GoBack()
        {
            activity.Finish();
        }

        public void GoMedicalInfoEdit(Ficha ficha)
        {
            Intent intent = new Intent(activity, typeof(MedicalInfoEditActivity));
            var FichaSerializable = JsonConvert.SerializeObject(ficha);
            intent.PutExtra("ficha", FichaSerializable);
            activity.StartActivity(intent);
        }    

        public void GoToForm()
        {
            Intent intent = new Intent(activity, typeof(MedicalTestActivity));
            intent.PutExtra("canBack", true);
            activity.StartActivity(intent);
            activity.Finish();
        }
    }
}