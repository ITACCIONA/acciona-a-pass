using System;
using System.Threading.Tasks;
using Acciona.Domain.Services;
using Acciona.Presentation.Navigation;
using Presentation.UI.Base;
using ServiceLocator;

namespace Acciona.Presentation.UI.Features.Language
{
    public class LanguagePresenter : BasePresenter<LanguageUI, ILanguageNavigator>
    {

        private ILocaleService localeService;
        private string configuredLang;
        private string newLang;

        public LanguagePresenter()
        {
            localeService = Locator.Current.GetService<ILocaleService>();
        }

        public override void OnCreate()
        {
            base.OnCreate();
            configuredLang = localeService.GetConfiguredLanguage();
            View.SetConfiguredLang(configuredLang);
        }

        public void BackClicked()
        {
            navigator.GoBack();
        }

        public void ModifyClicked()
        {
            if (newLang.Equals(configuredLang))
                navigator.GoBack();
            else
            {
                View.ShowDialog("language_confirm", "msg_cancel", "msg_ok", () =>
                {
                    localeService.SetLanguage(newLang, true);
                    navigator.RestarApp();
                });
            }
        }

        public void LangClicked(string lang)
        {
            newLang = lang;
            View.SetConfiguredLang(newLang);
        }
    }
}
