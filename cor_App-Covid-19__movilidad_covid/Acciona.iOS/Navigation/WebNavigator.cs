using System;
using Acciona.Presentation.Navigation;
using BaseIOS.Navigation;

namespace Acciona.iOS.Navigation
{
    public class WebNavigator : BaseNavigator, IWebNavigator
    {
        public void GoBack()
        {
            rootViewController.PopViewController(true);
        }
    }
}
