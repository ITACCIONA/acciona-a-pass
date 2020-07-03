using System;
using Acciona.Presentation.Navigation;
using BaseIOS.Navigation;

namespace Acciona.iOS.Navigation
{
    public class AlarmNavigator : BaseNavigator, IAlarmNavigator
    {
        public void GoBack()
        {
            rootViewController.PopViewController(true);
        }
    }
}
