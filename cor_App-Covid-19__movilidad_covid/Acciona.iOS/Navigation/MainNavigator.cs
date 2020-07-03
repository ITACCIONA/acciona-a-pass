using System;
using Acciona.iOS.UI.Features.Login;
using Acciona.iOS.UI.Features.Alarm;
using Acciona.Presentation.Navigation;
using BaseIOS.Navigation;
using Acciona.iOS.UI.Features.Alerts;
using Acciona.iOS.UI.Features.Health;

namespace Acciona.iOS.Navigation
{
    public class MainNavigator : BaseNavigator, IMainNavigator
    {
        public void GoToAlarm()
        {
            var viewController = new AlarmViewController();
            rootViewController.PushViewController(viewController, true);
        }

        public void GoToAlerts()
        {
            var viewController = new AlertsViewController();
            rootViewController.PushViewController(viewController, true);
        }

        public void GotToHealth()
        {
            var viewController = new HealthViewController();
            rootViewController.PushViewController(viewController, true);
        }
    }
}
