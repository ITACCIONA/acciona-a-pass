using System;
using Acciona.iOS.UI.Features.MedicalTest;
using Acciona.Presentation.Navigation;
using BaseIOS.Navigation;
using UIKit;

namespace Acciona.iOS.Navigation
{
    public class RegisteredNavigator : BaseNavigator, IRegisteredNavigator
    {
        public void GoToMedicalTest()
        {
            var viewController = new MedicalTestViewController();
            rootViewController.PushViewController(viewController, true);            
        }
    }
}
