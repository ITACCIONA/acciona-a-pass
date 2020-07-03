using System;
using Acciona.iOS.UI.Features.Splash;
using Acciona.Presentation.Navigation;
using BaseIOS.Navigation;
using UIKit;

namespace Acciona.iOS.Navigation
{
    public class LanguageNavigator : BaseNavigator, ILanguageNavigator
    {
        public void GoBack()
        {
            rootViewController.PopViewController(true);
        }

        public void RestarApp()
        {
            var viewController = new SplashViewController();
            rootViewController.SetViewControllers(new UIViewController[] { viewController }, true);
        }
    }
}
