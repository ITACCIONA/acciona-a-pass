using System;
using Acciona.Presentation.Navigation;
using BaseIOS.Navigation;

namespace Acciona.iOS.Navigation
{
    public class ContactDataNavigator : BaseNavigator, IContactDataNavigator
    {
        public void GoBack()
        {
            rootViewController.PopViewController(true);
        }
    }
}
