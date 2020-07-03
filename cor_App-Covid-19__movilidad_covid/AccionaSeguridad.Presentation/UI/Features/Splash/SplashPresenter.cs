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
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AccionaSeguridad.Presentation.UI.Features.Splash
{
    public class SplashPresenter : BasePresenter<SplashUI, ISplashNavigator>
    {
        private readonly GetVersionsUseCase getVersionsUseCase;
        private readonly ILocaleService localeService;
        private readonly ISettingsService settingsService;

        public SplashPresenter()
        {
            getVersionsUseCase = Locator.Current.GetService<GetVersionsUseCase>();
            localeService = Locator.Current.GetService<ILocaleService>();
            settingsService = Locator.Current.GetService<ISettingsService>();
        }

        public override void OnCreate()
        {
            base.OnCreate();
            ConfigureLanguage();
            CalculateNavigation();
            settingsService.AddOrUpdateValue(DomainConstants.LAST_START, DateTime.Now.ToString("dd/MM/yyyy"));
            var appSession = Locator.Current.GetService<AppSession>();
            string location = settingsService.GetValueOrDefault<string>(DomainConstants.LOCATION);
            if (location != null)
                appSession.Location = JsonConvert.DeserializeObject<Location>(location);

        }

        private void ConfigureLanguage()
        {
            string lang = localeService.GetConfiguredLanguage();
            bool manual = localeService.IsManualLanguage();
            if (lang.Length == 0 || !manual)
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
                localeService.SetLanguage(lang, true);
            }
        }

        public async Task CalculateNavigation()
        {
            await Task.Delay(2000);
            CheckVersion();
        }

        private async Task CheckVersion()
        {
            int version = View.GetVersion();
            var versions = await getVersionsUseCase.Execute();
            if (versions.ErrorCode > 0)
                navigator.GoToMain();
            else
            {
                if (version < versions.Data.SeguridadMinVersion)
                    View.ShowDialog("mandatory_update","msg_download",()=>View.Download());
                else if (version < versions.Data.SeguridadRecomendedVersion)
                    View.ShowDialog("recomended_update", "msg_continue", () => navigator.GoToMain(),"msg_download", () => View.Download());
                else
                    navigator.GoToMain();             
            }
        }
    }
}
