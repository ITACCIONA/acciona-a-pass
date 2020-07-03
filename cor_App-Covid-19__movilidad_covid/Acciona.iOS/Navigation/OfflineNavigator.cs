using System;
using Acciona.iOS.UI.Features.Login;
using Acciona.iOS.UI.Features.Main;
using Acciona.iOS.UI.Features.MedicalTest;
using Acciona.Presentation.Navigation;
using BaseIOS.Navigation;
using UIKit;

namespace Acciona.iOS.Navigation
{
    public class OfflineNavigator : BaseNavigator, IOfflineNavigator
    {
        public void GoToLogin()
        {
            var viewController = new LoginViewController();
            rootViewController.PushViewController(viewController , true);
            RemoveMeFromStack();            
        }

        public void GoToMain()
        {
            var viewController = new MainViewController();
            rootViewController.PushViewController( viewController , true);
            RemoveMeFromStack();
        }


    }
}
