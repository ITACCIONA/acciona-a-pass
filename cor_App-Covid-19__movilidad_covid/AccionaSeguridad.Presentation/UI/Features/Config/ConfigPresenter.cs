using System;
using System.Threading.Tasks;
using Acciona.Domain.Services;
using AccionaSeguridad.Presentation.Navigation;
using Presentation.UI.Base;
using ServiceLocator;

namespace Acciona.Presentation.UI.Features.Config
{
    public class ConfigPresenter : BasePresenter<ConfigUI, IConfigNavigator>
    {

        public ConfigPresenter()
        {            
        }

        public override void OnCreate()
        {
            base.OnCreate();         
        }

        public void BackClicked()
        {
            navigator.GoBack();
        }

        public void LanguageClicked()
        {
            navigator.GoLanguage();
        }

        public void CenterClicked()
        {
            navigator.GoCenter();
        }
    }
}
