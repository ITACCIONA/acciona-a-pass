using Acciona.Domain;
using Acciona.Domain.Model;
using Acciona.Domain.Model.Employee;
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

namespace Acciona.Presentation.UI.Features.AlertDetail
{
    public class AlertDetailPresenter : BasePresenter<AlertDetailUI, IAlertDetailNavigator>
    {
        private Alert alert;

        private MarkAlertReadUseCase markAlertReadUseCase;

        public AlertDetailPresenter()
        {
            markAlertReadUseCase = Locator.Current.GetService<MarkAlertReadUseCase>();
        }

        public override void OnCreate()
        {
            base.OnCreate();
            var appSession = Locator.Current.GetService<AppSession>();
            alert=appSession.SelectedAlert;
            View.SetAlert(alert);
            if (!alert.Read)
                MarkAlertRead();
        }

        private async Task MarkAlertRead()
        {
            //No importa si da error, es pero poner un mensaje que que tenga qyue entrar dos veces
            await markAlertReadUseCase.Execute(alert.IdAlert);
        }

        public void BackClicked()
        {
            navigator.GoBack();
        }
    }
}
