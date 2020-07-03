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

namespace Acciona.Presentation.UI.Features.Alerts
{
    public class AlertsPresenter : BasePresenter<AlertsUI, IAlertsNavigator>
    {
        private GetAlertsUseCase getAlertsUseCase;

        private IEnumerable<Alert> alerts;

        public AlertsPresenter()
        {            
            getAlertsUseCase = Locator.Current.GetService<GetAlertsUseCase>();
        }

        public override void OnResume()
        {
            base.OnResume();
            GetData();
        }

        private async Task GetData()
        {
            View.ShowLoading();
            var response = await getAlertsUseCase.Execute();
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
                alerts = response.Data;
                View.SetAlerts(alerts);
            }
        }

        public void BackClicked()
        {
            navigator.GoBack();
        }

        public void OpenAlert(Alert alert)
        {
            var appSession = Locator.Current.GetService<AppSession>();
            appSession.SelectedAlert = alert;
            navigator.GoToAlertDetail();
        }
    }
}
