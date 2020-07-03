using System;
using System.Collections.Generic;
using System.Linq;
using ServiceLocator;
using UIKit;

namespace BaseIOS.Navigation
{
    public class BaseNavigator
    {
        protected UIViewController controller;
        protected UINavigationController rootViewController
        {
            get => UIApplication.SharedApplication.KeyWindow.RootViewController as UINavigationController;
            set => UIApplication.SharedApplication.KeyWindow.RootViewController = value;
        }

        protected BaseNavigator()
        {
            controller = Locator.Current.GetService<UIViewController>();
            rootViewController = UIApplication.SharedApplication.KeyWindow.RootViewController as UINavigationController;
        }

        public void PopViewController()
        {
            if (rootViewController is UINavigationController)
            {
                ((UINavigationController)rootViewController).PopViewController(true);
            }
        }

        protected void RemoveMeFromStack()
        {
            List<UIViewController> all = rootViewController.ViewControllers.ToList();
            all.Remove(controller);
            rootViewController.ViewControllers = all.ToArray();
        }
    }
}
