using System;
using Acciona.iOS.UI.Features.AlertDetail;
using Acciona.iOS.UI.Features.Login;
using Acciona.Presentation.Navigation;
using BaseIOS.Navigation;

namespace Acciona.iOS.Navigation
{
    public class AlertsNavigator : BaseNavigator, IAlertsNavigator
    {
        public void GoBack()
        {
            rootViewController.PopViewController(true);
        }

        public void GoToAlertDetail()
        {
            var viewController = new AlertDetailViewController();
            rootViewController.PushViewController(viewController, true);
        }
    }
}
