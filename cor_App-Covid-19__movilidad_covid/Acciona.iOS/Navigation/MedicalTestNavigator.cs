using System;
using System.Collections.Generic;
using Acciona.iOS.UI.Features.Main;
using Acciona.Presentation.Navigation;
using BaseIOS.Navigation;
using UIKit;

namespace Acciona.iOS.Navigation
{
    public class MedicalTestNavigator : BaseNavigator, IMedicalTestNavigator
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
