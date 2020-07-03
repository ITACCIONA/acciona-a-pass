using System;
using Acciona.Presentation.Navigation;
using BaseIOS.Navigation;

namespace Acciona.iOS.Navigation
{
    public class WorkingCenterNavigator : BaseNavigator, IWorkingCenterNavigator
    {
        public void GoBack()
        {
            rootViewController.PopViewController(true);
        }
    }
}
