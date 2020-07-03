using Acciona.Domain;
using Acciona.Domain.Model;
using Acciona.Domain.Model.Employee;
using Acciona.Domain.Services;
using Acciona.Domain.UseCase;
using Acciona.Presentation.Navigation;
using Domain.Services;
using Newtonsoft.Json;
using Presentation.UI.Base;
using ServiceLocator;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Acciona.Presentation.UI.Features.Profile
{
    public class ProfilePresenter : BasePresenter<ProfileUI, IProfileNavigator>
    {
        private readonly GetFichaUseCase getFichaUseCase;
        private Ficha ficha;
        private LogoutUseCase logoutUseCase;
        private string errorMessage;

        public ProfilePresenter()
        {
            getFichaUseCase = Locator.Current.GetService<GetFichaUseCase>();
            logoutUseCase = Locator.Current.GetService<LogoutUseCase>();
        }

        public override void OnCreate()
        {
            base.OnCreate();
            GetData();
        }

        private async Task GetData()
        {
            View.ShowLoading();
            var response = await getFichaUseCase.Execute();
            View.HideLoading();
            if (response.ErrorCode > 0)
            {
                if (response.ErrorCode == 401)
                    View.ShowDialog(response.Message, "msg_ok", () => Locator.Current.GetService<ILogoutService>().LogoutExpired());
                else
                {
                    errorMessage = response.Message;
                    View.ShowDialog(response.Message, "msg_ok", null);
                }
            }
            else
            {
                ficha = response.Data;
                View.SetFicha(ficha);
            }
        }

        public void OpenMedicalInfo()
        {
            if(ficha==null)
                View.ShowDialog(errorMessage, "msg_ok", null);
            else
                navigator.GoToMedicalInfo(ficha);
        }

        public void OpenCenter()
        {
            navigator.GoToCenter(ficha);
        }

        public void LanguageClicked()
        {
            navigator.GoLanguage();
        }

        public void OpenContactData()
        {
            if (ficha == null)
                View.ShowDialog(errorMessage, "msg_ok", null);
            else
                navigator.GoToContactData(ficha);
        }


        public void LogoutClicked()
        {
            View.ShowDialog("disconnect_ask", "msg_no", "msg_yes", () =>
            {
                logoutUseCase.Execute();
                navigator.GoToLogin();
            });
        }
    }
}
