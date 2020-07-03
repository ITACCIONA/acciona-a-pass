using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Acciona.Domain;
using Acciona.Domain.Model;
using Acciona.Domain.Model.Employee;
using Acciona.Domain.Model.Security;
using Acciona.Domain.Services;
using Acciona.Domain.UseCase;
using Acciona.Presentation.Navigation;
using Domain.Services;
using Microsoft.AppCenter.Analytics;
using Newtonsoft.Json;
using Presentation.UI.Base;
using ServiceLocator;

namespace Acciona.Presentation.UI.Features.WorkingCenter
{
    public class WorkingCenterPresenter : BasePresenter<WorkingCenterUI, IWorkingCenterNavigator>
    {
        private readonly GetLocationsUseCase getLocationsUseCase;
        private readonly ModifyFichaUseCase modifyFichaUseCase;
        private IEnumerable<Location> locations;
        private Ficha ficha;
        private Location selected;

        public WorkingCenterPresenter()
        {
            getLocationsUseCase = Locator.Current.GetService<GetLocationsUseCase>();
            modifyFichaUseCase = Locator.Current.GetService<ModifyFichaUseCase>();
        }

        public override void OnCreate()
        {
            base.OnCreate();
            GetData();            
        }

        private async Task GetData()
        {
            await Task.Delay(100);
            View.ShowLoading();
            var response = await getLocationsUseCase.Execute(true);
            View.HideLoading();
            if (response.ErrorCode > 0)
            {
                View.ShowDialog(response.Message, "msg_ok", null);
            }
            else
            {                
                locations = response.Data;                
                selected = locations.FirstOrDefault(x => x.IdLocation == ficha.IdLocalizacion);
                View.SetSelectedLocation(selected);
                View.SetLocations(locations);
            }
        }

        public void SetFicha(Ficha ficha)
        {
            this.ficha = ficha;
        }

        public void BackClicked()
        {
            navigator.GoBack();
        }

        public async Task ModifyClicked(Location loc)
        {
            View.ShowLoading();
            long? idLocalizacion= loc.IdLocation == -1 ? (long?)null : loc.IdLocation;
            var response = await modifyFichaUseCase.Execute(ficha.TelefonoEmpleado, idLocalizacion);
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
                Analytics.TrackEvent("CentroTrabajoActualizado");
                ficha.IdLocalizacion = idLocalizacion;
                navigator.GoBack();
            }
            
        }
    }
}
