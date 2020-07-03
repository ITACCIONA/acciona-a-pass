using System;
using Acciona.Presentation.Navigation;
using BaseIOS.Navigation;

namespace Acciona.iOS.Navigation
{
    public class AlertDetailNavigator : BaseNavigator, IAlertDetailNavigator
    {
        public void GoBack()
        {
            rootViewController.PopViewController(true);
        }
    }
}
