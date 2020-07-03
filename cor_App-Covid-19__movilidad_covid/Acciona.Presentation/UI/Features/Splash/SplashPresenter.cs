using Acciona.Domain;
using Acciona.Domain.Model;
using Acciona.Domain.Model.Admin;
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

namespace Acciona.Presentation.UI.Features.Splash
{
    public class SplashPresenter : BasePresenter<SplashUI, ISplashNavigator>
    {

        public enum Platform
        {
            Android, iOS
        }

        private readonly GetPassportUseCase getPassportUseCase;
        private readonly GetOfflinePassportUseCase getOfflinePassportUseCase;
        //private readonly GetPassportStatesUseCase getPassportStatesUseCase;
        //private readonly GetPassportStatesColorsUseCase getPassportStatesColorsUseCase;        
        private readonly GetRiskFactorsUseCase getRiskFactorsUseCase;
        private readonly GetSymptomTypesUseCase getSymptomTypesUseCase;
        //private readonly GetMedicalMonitorsUseCase getMedicalMonitorsUseCase;
        private readonly GetVersionsUseCase getVersionsUseCase;
        private readonly ILocaleService localeService;

        public SplashPresenter()
        {
            getPassportUseCase = Locator.Current.GetService<GetPassportUseCase>();
            getOfflinePassportUseCase = Locator.Current.GetService<GetOfflinePassportUseCase>();
            //getPassportStatesUseCase = Locator.Current.GetService<GetPassportStatesUseCase>();
            //getPassportStatesColorsUseCase = Locator.Current.GetService<GetPassportStatesColorsUseCase>();            
            getRiskFactorsUseCase = Locator.Current.GetService<GetRiskFactorsUseCase>();
            getSymptomTypesUseCase = Locator.Current.GetService<GetSymptomTypesUseCase>();
            //getMedicalMonitorsUseCase = Locator.Current.GetService<GetMedicalMonitorsUseCase>();
            getVersionsUseCase = Locator.Current.GetService<GetVersionsUseCase>();
            localeService = Locator.Current.GetService<ILocaleService>();
        }

        public override void OnCreate()
        {
            base.OnCreate();
            ConfigureLanguage();
            CalculateNavigation();
        }

        private void ConfigureLanguage()
        {
            string lang = localeService.GetConfiguredLanguage();
            bool manual = localeService.IsManualLanguage();
            if (lang.Length==0 || !manual)
            {
                var languages = localeService.GetSupportedLanguages();
                AppSession appSession = Locator.Current.GetService<AppSession>();
                appSession.Language = "en";
                for (int i = 0; i < languages.Count; i++)
                {
                    if (languages[i].Equals("es"))
                    {
                        appSession.Language = "es";
                        break;
                    }
                    if (languages[i].Equals("en"))
                    {
                        appSession.Language = "en";
                        break;
                    }
                }               
                localeService.SetLanguage(appSession.Language);
            }
            else
            {
                Locator.Current.GetService<AppSession>().Language = lang;
                localeService.SetLanguage(lang,true);
            }
        }

        public async Task CalculateNavigation()
        {
            await Task.Delay(1500);
            CheckVersion();

        }

        public async Task CheckVersion()
        {
            Platform p = View.GetPlatform();
            int version = View.GetVersion();
            var versions = await getVersionsUseCase.Execute();
            if(versions.ErrorCode>0)
                await GoLogin();
            else
            {
                switch (p)
                {
                    case Platform.Android:
                        if (version < versions.Data.AndroidMinVersion)
                            View.ShowDialog("mandatory_update", "msg_download",()=>View.DownloadApp());
                        else if (version < versions.Data.AndroidRecomendedVersion)
                            View.ShowDialog("recomended_update", "msg_continue", ()=> GoLogin(), "msg_download", () => View.DownloadApp());
                        else
                            await GoLogin();
                        break;
                    case Platform.iOS:
                        if (version < versions.Data.IosMinVersion)
                            View.ShowDialog("mandatory_update","msg_download", () => View.DownloadApp());
                        else if (version < versions.Data.IosRecomendedVersion)
                            View.ShowDialog("recomended_update", "msg_continue", () => GoLogin(), "msg_download", () => View.DownloadApp());
                        else
                            await GoLogin();
                        break;
                }
            }
        }

        private async Task GoLogin()
        {
            var settingsService = Locator.Current.GetService<ISettingsService>();
            long ticks = settingsService.GetValueOrDefault<long>(DomainConstants.LAST_DATE, 0);
            if (ticks == 0)
            {
                navigator.GoToLogin();
                return;
            }
            /*DateTime last = new DateTime(ticks);
            if (!DateTime.Now.ToString("yyyy-MM-dd").Equals(last.ToString("yyyy-MM-dd")))
            {
                navigator.GoToLogin();
                return;
            }*/
            String token = settingsService.GetValueOrDefault<string>(DomainConstants.LAST_SESSION);
            if (token == null || token.Trim().Equals(""))
            {
                navigator.GoToLogin();
                return;
            }
            var appSession = Locator.Current.GetService<AppSession>();
            appSession.AccesToken = token;
            appSession.User = settingsService.GetValueOrDefault<string>(DomainConstants.LAST_USER);
            string userInfo = settingsService.GetValueOrDefault<string>(DomainConstants.LAST_USER_INFO);
            if (userInfo != null)
                appSession.UserInfo = JsonConvert.DeserializeObject<User>(userInfo);
            View.ShowLoading();
            var passportResponse = await getPassportUseCase.Execute(true);
            //await getPassportStatesUseCase.Execute(true);
            //await getPassportStatesColorsUseCase.Execute(true);            
            await getRiskFactorsUseCase.Execute(true);
            await getSymptomTypesUseCase.Execute(true);
            //await getMedicalMonitorsUseCase.Execute(true);
            View.HideLoading();
            //navigator.GoToOffline();
            //return;
            if (passportResponse.ErrorCode > 0)
            {
                if (passportResponse.ErrorCode == 401)
                {
                    navigator.GoToLogin();
                }
                else if (passportResponse.ErrorCode == 500)
                {
                    navigator.GoToLogin();
                }
                else
                {
                    var passport = getOfflinePassportUseCase.Execute();
                    if(passport==null)
                        navigator.GoToLogin();                    
                    else
                    {                        
                        navigator.GoToOffline();
                    }                    
                }
            }
            else
            {
                if (passportResponse.Data == null)
                    navigator.GoToLogin();// En el login tenemos botón de reintentar  navigator.GoToMedicalTest();
                else
                    navigator.GoToMain();
            }
        }
    }
}
