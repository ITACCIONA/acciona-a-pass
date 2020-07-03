using System;
using Acciona.Presentation.Navigation;
using BaseIOS.Navigation;

namespace Acciona.iOS.Navigation
{
    public class HealthNavigator : BaseNavigator, IHealthNavigator
    {
        public void GoBack()
        {
            rootViewController.PopViewController(true);
        }
    }
}
