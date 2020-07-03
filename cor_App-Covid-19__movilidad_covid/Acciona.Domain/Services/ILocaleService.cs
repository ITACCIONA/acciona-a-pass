using System;
using System.Collections.Generic;

namespace Acciona.Domain.Services
{
    public interface ILocaleService
    {
        List<string> GetSupportedLanguages();
        void SetLanguage(string lang,bool manual=false);
        string GetConfiguredLanguage();
        bool IsManualLanguage();
    }
}
