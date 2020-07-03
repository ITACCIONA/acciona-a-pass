using System;
using System.Threading.Tasks;
using Acciona.Domain.Model.Employee;
using Acciona.Domain.Services;
using Acciona.Domain.UseCase;
using Acciona.Presentation.Navigation;
using Microsoft.AppCenter.Analytics;
using Presentation.UI.Base;
using ServiceLocator;

namespace Acciona.Presentation.UI.Features.DataContact
{
    public class ContactDataPresenter : BasePresenter<ContactDataUI, IContactDataNavigator>
    {       
        public Ficha ficha;

        private readonly ModifyFichaUseCase modifyFichaUseCase;

        public ContactDataPresenter()
        {
            modifyFichaUseCase = Locator.Current.GetService<ModifyFichaUseCase>();
        }

        public override void OnCreate()
        {
            base.OnCreate();
            View.setProfileData(ficha);
        }

        public void BackClicked()
        {
            navigator.GoBack();
        }

        public async Task SaveClicked(string phone)
        {

            View.ShowLoading();
            var response = await modifyFichaUseCase.Execute(phone,ficha.IdLocalizacion);
            View.HideLoading();
            if (response.ErrorCode > 0)
            {
                if (response.ErrorCode == 401)
                    View.ShowDialog(response.Message, "msg_ok", () => Locator.Current.GetService<ILogoutService>().LogoutExpired());
                else
                    View.ShowDialog(response.Message, "msg_ok", null);
            }
            else
            {
                Analytics.TrackEvent("TelefonoActualizado");
                ficha.TelefonoEmpleado = phone;
                View.ShowDialog("contact_data_save_modify_data_ok", "msg_ok", null);
            }
        }
    }
}
