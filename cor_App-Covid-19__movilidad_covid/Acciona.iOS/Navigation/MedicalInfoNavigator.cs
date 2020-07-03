using System;
using Acciona.Domain.Model.Employee;
using Acciona.iOS.UI.Features.MedicalInfoEdit;
using Acciona.iOS.UI.Features.MedicalTest;
using Acciona.Presentation.Navigation;
using BaseIOS.Navigation;

namespace Acciona.iOS.Navigation
{
    public class MedicalInfoNavigator : BaseNavigator, IMedicalInfoNavigator
    {
        public void GoBack()
        {
            rootViewController.PopViewController(true);
        }

        public void GoMedicalInfoEdit(Ficha ficha)
        {
            var viewController = new MedicalInfoEditViewController(ficha);
            rootViewController.PushViewController(viewController, true);
        }

        public void GoToForm()
        {
            var viewController = new MedicalTestViewController(true);
            rootViewController.PushViewController(viewController, true);
            
        }
    }
}
