using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Acciona.Domain;
using Acciona.Domain.Model;
using Acciona.Domain.Model.Security;
using Acciona.Domain.Services;
using Acciona.Domain.UseCase;
using AccionaSeguridad.Presentation.Navigation;
using Domain.Services;
using Newtonsoft.Json;
using Presentation.UI.Base;
using ServiceLocator;

namespace Acciona.Presentation.UI.Features.WorkingCenter
{
    public class WorkingCenterPresenter : BasePresenter<WorkingCenterUI, IWorkingCenterNavigator>
    {
        private readonly GetSecurityLocationsUseCase getSecurityLocationsUseCase;
        private readonly ISettingsService settingsService;
        private AppSession appSession;
        private IEnumerable<Location> locations;
        

        public WorkingCenterPresenter()
        {
            getSecurityLocationsUseCase = Locator.Current.GetService<GetSecurityLocationsUseCase>();
            settingsService = Locator.Current.GetService<ISettingsService>();
            appSession = Locator.Current.GetService<AppSession>();
        }

        public override void OnCreate()
        {
            base.OnCreate();
            GetData();
            View.SetSelectedLocation(appSession.Location);
        }

        private async Task GetData()
        {
            View.ShowLoading();
            var response = await getSecurityLocationsUseCase.Execute();
            View.HideLoading();
            if (response.ErrorCode > 0)
            {
                View.ShowDialog(response.Message, "msg_ok", null);
            }
            else
            {                
                locations = response.Data;
                View.SetLocations(locations);
            }
        }

        public void BackClicked()
        {
            navigator.GoBack();
        }

        public void ModifyClicked(Location loc)
        {
            appSession.Location = loc;
            settingsService.AddOrUpdateValue(DomainConstants.LOCATION, JsonConvert.SerializeObject(loc));
            navigator.GoBack();
        }
    }
}
