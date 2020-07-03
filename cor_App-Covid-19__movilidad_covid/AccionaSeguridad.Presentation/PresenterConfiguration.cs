using ServiceLocator;
using AccionaSeguridad.Presentation.UI.Features.Login;
using AccionaSeguridad.Presentation.UI.Features.Main;
using AccionaSeguridad.Presentation.UI.Features.Splash;
using AccionaSeguridad.Presentation.UI.Features.Result;
using AccionaSeguridad.Presentation.UI.Features.Manual;
using AccionaSeguridad.Presentation.UI.Features.ManualResult;
using Acciona.Presentation.UI.Features.Language;
using Acciona.Presentation.UI.Features.Config;
using Acciona.Presentation.UI.Features.WorkingCenter;

namespace AccionaSeguridad.Presentation
{
    public static class PresenterConfiguration
    {
        public static void Init()
        {
            //Presenters
            Locator.CurrentMutable.Register(() => new SplashPresenter());
            Locator.CurrentMutable.Register(() => new LoginPresenter());
            Locator.CurrentMutable.Register(() => new MainPresenter());
            Locator.CurrentMutable.Register(() => new ResultPresenter());
            Locator.CurrentMutable.Register(() => new ManualPresenter());
            Locator.CurrentMutable.Register(() => new ManualResultPresenter());
            Locator.CurrentMutable.Register(() => new LanguagePresenter());
            Locator.CurrentMutable.Register(() => new ConfigPresenter());
            Locator.CurrentMutable.Register(() => new WorkingCenterPresenter());
        }
    }
}
