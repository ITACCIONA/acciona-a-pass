using System;
using Acciona.iOS.UI.Features.Health;
using Acciona.Presentation.Navigation;
using BaseIOS.Navigation;

namespace Acciona.iOS.Navigation
{
    public class QRcodeNavigator : BaseNavigator, IQRcodeNavigator
    {
        public void GotToHealth()
        {
            var viewController = new HealthViewController();
            rootViewController.PushViewController(viewController, true);
        }
    }
}
