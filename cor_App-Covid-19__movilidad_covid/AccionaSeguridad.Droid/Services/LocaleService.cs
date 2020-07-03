using System;
using System.Collections.Generic;
using Acciona.Domain.Services;
using Android.App;
using Android.Content.Res;
using Android.OS;
using Android.Support.V4.OS;
using Domain.Services;
using Java.Util;
using ServiceLocator;

namespace AccionaSeguridad.Droid.Services
{
    public class LocaleService:ILocaleService
    {
        private const string STORE_LANG = "StoreLang";
        private const string MANUAL_LANG = "ManualLang";

        public bool IsManualLanguage()
        {
            var settingsService = Locator.Current.GetService<ISettingsService>();
            return settingsService.GetValueOrDefault<bool>(MANUAL_LANG, false);
        }

        public string GetConfiguredLanguage()
        {
            var settingsService = Locator.Current.GetService<ISettingsService>();
            return settingsService.GetValueOrDefault<string>(STORE_LANG, "");
        }

        public List<string> GetSupportedLanguages()
        {
            var activity = Locator.Current.GetService<Activity>();
            LocaleListCompat locales = ConfigurationCompat.GetLocales(activity.Resources.Configuration);
            List<String> localLanguages = new List<String>();
            for (int i = 0; i < locales.Size(); i++)
            {
                localLanguages.Add(locales.Get(i).Language);
            }
            return localLanguages;
        }

        public void SetLanguage(string lang,bool manual=false)
        {
            var settingsService = Locator.Current.GetService<ISettingsService>();
            settingsService.AddOrUpdateValue(STORE_LANG, lang);
            settingsService.AddOrUpdateValue(MANUAL_LANG, manual);

            var activity = Locator.Current.GetService<Activity>();

            Locale locale = new Locale(lang);
            Locale.Default = locale;

            Resources res = activity.Resources;
            Configuration config = new Configuration(res.Configuration);
            /*if ((int)Build.VERSION.SdkInt >= 17)
            {
                config.Locale = locale;
                var context=activity.CreateConfigurationContext(config);
            }
            else
            {*/
                config.Locale = locale;
                res.Configuration.Locale = locale;
                res.UpdateConfiguration(config, res.DisplayMetrics);
                
            //}                        
        }
    }
}
