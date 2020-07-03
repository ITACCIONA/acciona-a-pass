using System;
using Acciona.iOS.UI.Features.Main;
using Acciona.iOS.UI.Features.MedicalTest;
using Acciona.iOS.UI.Features.Registered;
using Acciona.Presentation.Navigation;
using BaseIOS.Navigation;
using UIKit;

namespace Acciona.iOS.Navigation
{
    public class LoginNavigator : BaseNavigator, ILoginNavigator
    {
        public void GoToMain()
        {
            var viewController = new MainViewController();
            rootViewController.PushViewController( viewController , true);
            RemoveMeFromStack();
        }

        public void GoToMedicalTest()
        {
            var viewController = new MedicalTestViewController();
            rootViewController.PushViewController(viewController, true);
            RemoveMeFromStack();
        }

        public void GoToRegistered()
        {
            var viewController = new RegisteredViewController();
            rootViewController.PushViewController( viewController , true);
            RemoveMeFromStack();
        }
    }
}
