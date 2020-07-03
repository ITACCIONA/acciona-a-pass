using System;
using Acciona.Domain.Model.Employee;
using Acciona.iOS.UI.Features.ContactData;
using Acciona.iOS.UI.Features.Language;
using Acciona.iOS.UI.Features.Login;
using Acciona.iOS.UI.Features.MedicalInfo;
using Acciona.iOS.UI.Features.WorkingCenter;
using Acciona.Presentation.Navigation;
using BaseIOS.Navigation;
using UIKit;

namespace Acciona.iOS.Navigation
{
    public class ProfileNavigator : BaseNavigator, IProfileNavigator
    {
        public void GoToContactData(Ficha ficha)
        {
            var viewController = new ContactDataViewController(ficha);
            rootViewController.PushViewController(viewController, true);
        }

        public void GoToMedicalInfo(Ficha ficha)
        {
            var viewController = new MedicalInfoViewController(ficha);
            rootViewController.PushViewController(viewController, true);
        }

        public void GoToLogin()
        {
            var viewController = new LoginViewController();
            rootViewController.SetViewControllers(new UIViewController[] { viewController }, true);
          
        }

        public void GoLanguage()
        {
            var viewController = new LanguageViewController();
            rootViewController.PushViewController(viewController, true);
        }

        public void GoToCenter(Ficha ficha)
        {
            var viewController = new WorkingCenterViewController(ficha);
            rootViewController.PushViewController(viewController, true);
        }
    }
}
