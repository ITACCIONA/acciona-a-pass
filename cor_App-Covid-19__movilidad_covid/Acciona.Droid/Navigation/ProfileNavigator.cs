using Android.Content;
using Acciona.Presentation.Navigation;
using Acciona.Droid.UI.Features.Login;
using Droid.Navigation;
using Acciona.Domain.Model.Employee;
using Acciona.Droid.UI.Features.ContactData;
using Acciona.Droid.UI.Features.MedicalInfo;
using Android.OS;
using Newtonsoft.Json;
using Acciona.Droid.UI.Features.Language;
using Acciona.Droid.UI.Features.WorkingCenter;

namespace Acciona.Droid.Navigation
{
    public class ProfileNavigator : BaseNavigator, IProfileNavigator
    {
        public void GoToContactData(Ficha ficha)
        {
            Intent intent = new Intent(activity, typeof(ContactDataActivity));
            var FichaSerializable = JsonConvert.SerializeObject(ficha);
            intent.PutExtra("ficha", FichaSerializable);          
            activity.StartActivity(intent);

        }

        public void GoToMedicalInfo(Ficha ficha)
        {
            Intent intent = new Intent(activity, typeof(MedicalInfoActivity));
            var FichaSerializable = JsonConvert.SerializeObject(ficha);
            intent.PutExtra("ficha", FichaSerializable);
            activity.StartActivity(intent);
        }

        public void GoToLogin()
        {
            Intent intent = new Intent(activity, typeof(LoginActivity));
            activity.StartActivity(intent);
            activity.Finish();
        }

        public void GoLanguage()
        {
            Intent intent = new Intent(activity, typeof(LanguageActivity));
            activity.StartActivity(intent);
        }

        public void GoToCenter(Ficha ficha)
        {
            Intent intent = new Intent(activity, typeof(WorkingCenterActivity));
            var FichaSerializable = JsonConvert.SerializeObject(ficha);
            intent.PutExtra("ficha", FichaSerializable);
            activity.StartActivity(intent);
        }
    }
}