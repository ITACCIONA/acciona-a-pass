using System;
using Acciona.Domain.Services;
using Acciona.iOS.UI.Features.Login;
using UIKit;

namespace Acciona.iOS.Services
{
    public class LogoutService : ILogoutService
    {
        public void LogoutExpired()
        {
            var rootViewController = UIApplication.SharedApplication.KeyWindow.RootViewController as UINavigationController;
            var viewController = new LoginViewController();
            rootViewController.SetViewControllers(new UIViewController[] { viewController }, true);
        }
    }
}
