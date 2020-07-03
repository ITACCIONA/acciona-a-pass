using Acciona.Domain;
using Acciona.Domain.Model;
using Acciona.Domain.Services;
using Acciona.Presentation.Navigation;
using Domain.Services;
using Newtonsoft.Json;
using Presentation.UI.Base;
using ServiceLocator;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Acciona.Presentation.UI.Features.Alarm
{
    public class AlarmPresenter : BasePresenter<AlarmUI, IAlarmNavigator>
    {        
        public override void OnCreate()
        {
            base.OnCreate();                  
        }

        public void BackClicked()
        {
            navigator.GoBack();
        }

        public void CallClicked(string v)
        {
            
        }

        public void OpenSkypeEnterprise()
        {
          
            var shareService = Locator.Current.GetService<IShareService>();
            shareService.ShareApp(TargetApp.SkypeEnterprise);
        }

        public void ContactEmailClicked()
        {
           
            var shareService = Locator.Current.GetService<IShareService>();
            if (!shareService.ShareApp(TargetApp.Email))
            {
                View.ShowDialog("no_mail_app", "msg_ok", null);
            }
        }



    }
}
