using System;
using System.Collections.Generic;
using System.Linq;
using Acciona.Domain.Services;
using Domain.Services;
using Foundation;
using ServiceLocator;

namespace Acciona.iOS.Services
{
    public class LocaleService:ILocaleService
    {
        public LocaleService()
        {
        }

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
            var languages = NSLocale.PreferredLanguages;

            return languages.Select(x=>x.Substring(0,2)).ToList();
        }


        public void SetLanguage(string lang, bool manual = false)
        {
            var settingsService = Locator.Current.GetService<ISettingsService>();
            settingsService.AddOrUpdateValue(STORE_LANG, lang);
            settingsService.AddOrUpdateValue(MANUAL_LANG, manual);

            NSUserDefaults.StandardUserDefaults.SetValueForKey(NSArray.FromStrings(lang), new NSString("AppleLanguages"));
            NSUserDefaults.StandardUserDefaults.Synchronize();
            if (lang.Equals("en"))
            {
                var path = NSBundle.MainBundle.PathForResource("Base", "lproj");
                AppDelegate.LanguageBundle = NSBundle.FromPath(path);
            }
            else
            {
                var path = NSBundle.MainBundle.PathForResource(lang, "lproj");
                AppDelegate.LanguageBundle = NSBundle.FromPath(path);
            }

        }       
    }
}
