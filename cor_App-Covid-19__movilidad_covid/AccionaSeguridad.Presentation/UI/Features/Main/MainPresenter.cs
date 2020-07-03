using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Acciona.Domain;
using Acciona.Domain.Model;
using Acciona.Domain.Services;
using Acciona.Domain.UseCase;
using Acciona.Domain.Utils;
using AccionaSeguridad.Presentation.Navigation;
using Domain.Services;
using Messenger;
using Newtonsoft.Json;
using Presentation.UI.Base;
using ServiceLocator;

namespace AccionaSeguridad.Presentation.UI.Features.Main
{
    public class MainPresenter : BasePresenter<MainUI, IMainNavigator>
    {        
        private readonly IMessenger messenger;
        private readonly ILocaleService localeService;
        private readonly ISettingsService settingsService;

        public MainPresenter()
        {            
            messenger = Locator.Current.GetService<IMessenger>();     
            localeService = Locator.Current.GetService<ILocaleService>();
            settingsService = Locator.Current.GetService<ISettingsService>();
        }

        public override void OnCreate()
        {
            base.OnCreate();            
        }

        public override void OnResume()
        {
            base.OnResume();
            string date = DateTime.Now.ToString("dd/MM/yyyy");
            var start = settingsService.GetValueOrDefault(DomainConstants.LAST_START, "");
            if (!date.Equals(start))
                navigator.GoSplash();
            AppSession appSession = Locator.Current.GetService<AppSession>();
            if (appSession.Location != null)
                View.SetSelectedLocation(appSession.Location);
            else
                navigator.OpenCenter();
        }

        public override void OnDestroy()
        {            
            base.OnDestroy();
        }

        public void ScreenTouched()
        {
            View.ShowQRScanner();
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

        public void CenterClicked()
        {
            navigator.OpenCenter();
        }

        public async void SetSacnnerResult(string text)
        {
            ConfigureLanguage();
            if (text != null)
            {
                try
                {
                    //var info = JsonConvert.DeserializeObject<QRInfo>(text);
                    var encripted = EncryptUtils.Desencriptar(text);
                    var values = encripted.Split(';');
                    var info = new QRInfo()
                    {
                        IdEmpleado=Convert.ToInt32(values[0]),                        
                        PasaporteColor=values[1],                        
                    };
                    long date = Convert.ToInt64(values[2]);
                    if (date >= 0)
                        info.FechaExpiracion = new DateTime(date);
                    Locator.Current.GetService<AppSession>().QRInfo = info;
                    navigator.GoResult();
                }catch(Exception e)
                {
                    await Task.Delay(500);
                    View.ShowDialog("qr_no_valid", "msg_ok", null);
                }
            }
        }

        public void ManualClicked()
        {
            navigator.GoManual();
        }

        public void ConfigClicked()
        {
            navigator.GoConfig();
        }
    }
}
