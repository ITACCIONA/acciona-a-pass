using Acciona.Domain;
using Acciona.Domain.Model;
using Acciona.Domain.Model.Security;
using Acciona.Domain.UseCase;
using AccionaSeguridad.Presentation.Navigation;
using Domain.Services;
using Newtonsoft.Json;
using Presentation.UI.Base;
using ServiceLocator;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AccionaSeguridad.Presentation.UI.Features.ManualResult
{
    public class ManualResultPresenter : BasePresenter<ManualResultUI, IManualResultNavigator>
    {
        private readonly GenerateManualUseCase generateManualUseCase;

        private UserPaper user;

        public ManualResultPresenter()
        {
            generateManualUseCase = Locator.Current.GetService<GenerateManualUseCase>();
        }

        public override void OnCreate()
        {
            base.OnCreate();
            View.SetName(user.NombreEmpleado + " " + user.ApellidosEmpleado);
        }

        public void BackClicked()
        {
            View.Close();
        }

        public void SetUser(UserPaper user)
        {
            this.user = user;
        }

        public async Task NoClicked()
        {
            var info = new QRInfo()
            {
                IdEmpleado = user.IdEmpleado,
                PasaporteColor = "Verde",
                Manual=true
            };            
            Locator.Current.GetService<AppSession>().QRInfo = info;
            View.ShowLoading();
            var response = await generateManualUseCase.Execute(user.IdEmpleado, true);
            View.HideLoading();
            if (response.ErrorCode > 0)
                View.ShowDialog(response.Message, "msg_ok", null);
            else
                navigator.GoResult();
        }

        public async Task YesClicked()
        {
            var info = new QRInfo()
            {
                IdEmpleado = user.IdEmpleado,
                PasaporteColor = "Rojo",
                Manual = true
            };
            Locator.Current.GetService<AppSession>().QRInfo = info;
            View.ShowLoading();
            var response = await generateManualUseCase.Execute(user.IdEmpleado, false);
            View.HideLoading();
            if (response.ErrorCode > 0)
                View.ShowDialog(response.Message, "msg_ok", null);
            else
                navigator.GoResult();
        }
    }
}
