using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Acciona.Domain.Model;
using Acciona.Domain.Services;
using Acciona.Domain.UseCase;
using Acciona.Presentation.Navigation;
using Messenger;
using Microsoft.AppCenter.Analytics;
using Presentation.UI.Base;
using ServiceLocator;
using static Acciona.Presentation.UI.Features.Splash.SplashPresenter;

namespace Acciona.Presentation.UI.Features.Main
{
    public class MainPresenter : BasePresenter<MainUI, IMainNavigator>
    {
        private readonly GetAlertsUseCase getAlertsUseCase;
        private readonly GetVersionsUseCase getVersionsUseCase;

        public enum MainState
        {
            PASSPORT,  PROFILE
        }

        private MainState state = MainState.PASSPORT;

        private IMessenger messenger;        

        public MainPresenter()
        {            
            messenger = Locator.Current.GetService<IMessenger>();
            getAlertsUseCase = Locator.Current.GetService<GetAlertsUseCase>();
            getVersionsUseCase = Locator.Current.GetService<GetVersionsUseCase>();
        }

        public override void OnCreate()
        {
            base.OnCreate();            
        }

        public override void OnResume()
        {
            base.OnResume();            
            CheckVersion();
            GetData();
            GetNotifications();            
        }        

        public async Task CheckVersion()
        {
            Platform p = View.GetPlatform();
            int version = View.GetVersion();
            var versions = await getVersionsUseCase.Execute();
            if (versions.ErrorCode > 0)
            {
                //await GoLogin();
            }
            else
            {                
                switch (p)
                {
                    case Platform.Android:
                        if (version < versions.Data.AndroidMinVersion)
                            View.ShowDialog("mandatory_update", "msg_download", () => View.DownloadApp());
                        else if (version < versions.Data.AndroidRecomendedVersion)
                            View.ShowDialog("recomended_update", "msg_continue", null, "msg_download", () => View.DownloadApp());
                        break;
                    case Platform.iOS:
                        if (version < versions.Data.IosMinVersion)
                            View.ShowDialog("mandatory_update", "msg_download", () => View.DownloadApp());
                        else if (version < versions.Data.IosRecomendedVersion)
                            View.ShowDialog("recomended_update", "msg_continue", null, "msg_download", () => View.DownloadApp());
                        break;
                }
            }
        }

        private async Task GetNotifications()
        {
            View.ShowLoading();            
            var response = await getAlertsUseCase.Execute();
            View.HideLoading();
            if (response.ErrorCode > 0)
            {
                if (response.ErrorCode == 401)
                    View.ShowDialog(response.Message, "msg_ok", ()=>Locator.Current.GetService<ILogoutService>().LogoutExpired());
                else
                    alertIconNotRead(false);
            }
            else
            {
                var alerts = response.Data;
                var alert = alerts.Where(x => !x.Read).ToList();
                if (alert.Count > 0)
                {
                    alertIconNotRead(true);
                }
                else
                {
                    alertIconNotRead(false);
                }
            }
        }

        private void alertIconNotRead(bool notRead)
        {
            View.ShowNotificationsNotRead(notRead);
        }

        private async Task GetData()
        {
            await Task.Delay(50); //Pantalla este construida
            View.ShowState(state);
        }

        public void PassportClicked()
        {
            state = MainState.PASSPORT;
            View.ShowState(state);
        }
        

        public void ProfileClicked()
        {
            state = MainState.PROFILE;
            View.ShowState(state);
        }

        public void PhoneClicked()
        {
            navigator.GoToAlarm();
        }

        public void BellClicked()
        {
            navigator.GoToAlerts();
        }

        public void AlarmClicked()
        {            
            Locator.Current.GetService<AppSession>().Panic = true;
            navigator.GotToHealth();
        }
    }
}
