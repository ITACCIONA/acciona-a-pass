using System;
using Acciona.iOS.UI.Features.Main;
using Acciona.Presentation.Navigation;
using BaseIOS.Navigation;
using UIKit;

namespace Acciona.iOS.Navigation
{
    public class MedicalInfoEditNavigator : BaseNavigator, IMedicalInfoEditNavigator
    {
        public void GoBack()
        {
            rootViewController.PopViewController(true);
        }

        public void GoToMain()
        {
            var viewController = new MainViewController();
            rootViewController.SetViewControllers(new UIViewController[] { viewController }, true);
        }
    }
}
