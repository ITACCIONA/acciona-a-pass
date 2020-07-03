using ServiceLocator;
using Acciona.Presentation.UI.Features.Login;
using Acciona.Presentation.UI.Features.Main;
using Acciona.Presentation.UI.Features.Splash;
using Acciona.Presentation.UI.Features.Passport;
using Acciona.Presentation.UI.Features.Profile;
using Acciona.Presentation.UI.Features.QRcode;
using Acciona.Presentation.UI.Features.Alarm;
using Acciona.Presentation.UI.Features.Alerts;
using Acciona.Presentation.UI.Features.AlertDetail;
using Acciona.Presentation.UI.Features.Health;
using Acciona.Presentation.UI.Features.DataContact;
using Acciona.Presentation.UI.Features.MedicalInfo;
using Acciona.Presentation.UI.Features.MedicalTest;
using Acciona.Presentation.UI.Features.Registered;
using Acciona.Presentation.UI.Features.MedicalInfoEdit;
using Acciona.Presentation.UI.Features.Web;
using Acciona.Presentation.UI.Features.Offline;
using Acciona.Presentation.UI.Features.Language;
using Acciona.Presentation.UI.Features.WorkingCenter;

namespace Acciona.Presentation
{
    public static class PresenterConfiguration
    {
        public static void Init()
        {
            //Presenters
            Locator.CurrentMutable.Register(() => new SplashPresenter());
            Locator.CurrentMutable.Register(() => new LoginPresenter());
            Locator.CurrentMutable.Register(() => new MainPresenter());
            Locator.CurrentMutable.Register(() => new PassportPresenter());
            Locator.CurrentMutable.Register(() => new ProfilePresenter());
            Locator.CurrentMutable.Register(() => new QRcodePresenter());
            Locator.CurrentMutable.Register(() => new AlarmPresenter());
            Locator.CurrentMutable.Register(() => new AlertsPresenter());
            Locator.CurrentMutable.Register(() => new AlertDetailPresenter());
            Locator.CurrentMutable.Register(() => new HealthPresenter());
            Locator.CurrentMutable.Register(() => new ContactDataPresenter());
            Locator.CurrentMutable.Register(() => new MedicalInfoPresenter());
            Locator.CurrentMutable.Register(() => new MedicalTestPresenter());
            Locator.CurrentMutable.Register(() => new MedicalInfoEditPresenter());
            Locator.CurrentMutable.Register(() => new RegisteredPresenter());
            Locator.CurrentMutable.Register(() => new WebPresenter());
            Locator.CurrentMutable.Register(() => new OfflinePresenter());
            Locator.CurrentMutable.Register(() => new LanguagePresenter());
            Locator.CurrentMutable.Register(() => new WorkingCenterPresenter());
        }
    }
}
