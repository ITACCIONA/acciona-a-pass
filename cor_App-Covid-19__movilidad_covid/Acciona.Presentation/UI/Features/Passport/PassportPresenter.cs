using Acciona.Domain;
using Acciona.Domain.Model;
using Acciona.Presentation.Navigation;
using Domain.Services;
using Newtonsoft.Json;
using Presentation.UI.Base;
using ServiceLocator;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Acciona.Presentation.UI.Features.Passport
{
    public class PassportPresenter : BasePresenter<PassportUI, IPassportNavigator>
    {        
        public override void OnCreate()
        {
            base.OnCreate();                  
        }                
    }
}
