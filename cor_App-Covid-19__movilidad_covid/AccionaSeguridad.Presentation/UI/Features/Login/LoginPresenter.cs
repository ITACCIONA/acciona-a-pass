
using System;
using System.Linq;
using System.Threading.Tasks;
using Acciona.Domain;
using Acciona.Domain.Model;
using Acciona.Domain.Services;
using Acciona.Domain.UseCase;
using AccionaSeguridad.Presentation.Navigation;
using Domain.Services;
using Newtonsoft.Json;
using Presentation.UI.Base;
using ServiceLocator;

namespace AccionaSeguridad.Presentation.UI.Features.Login
{
    public class LoginPresenter : BasePresenter<LoginUI, ILoginNavigator>
    {                
        private readonly ISettingsService settingsService;
        private string oldUser;

        public LoginPresenter()
        {            
            settingsService = Locator.Current.GetService<ISettingsService>();
        }

        public override void OnCreate()
        {
            base.OnCreate();
            oldUser = settingsService.GetValueOrDefault<string>(DomainConstants.LAST_USER, "");
            View.SetLastUser(oldUser);
        }

        public void LoginClicked(string user, string pass)
        {            
            MakeLoginAsync(user, pass);
        }

        private async Task MakeLoginAsync(string user, string pass)
        {
            //if (!View.CheckInternet())
            //{
            //    View.ShowDialog("no_connection", "msg_ok", null);
            //    return;
            //}
            //View.ShowLoading();
            //var loginResponse = await loginUseCase.Execute(user.Trim(), pass.Trim());
            //if (loginResponse.ErrorCode > 0)
            //{
            //    View.ShowDialog(loginResponse.Message, "msg_ok", null);
            //}
            //else
            //{
            //        if (!user.Equals(oldUser))
            //            settingsService.AddOrUpdateValue<long>(DomainConstants.LAST_DATA_UPDATE, -1); //Frzar descarga de datos para el nuevo usuario
            //        var appSession = Locator.Current.GetService<AppSession>();
            //        settingsService.AddOrUpdateValue<string>(DomainConstants.LAST_SESSION, appSession.AccesToken);
            //        settingsService.AddOrUpdateValue<string>(DomainConstants.LAST_USER, appSession.User);
            //        settingsService.AddOrUpdateValue<long>(DomainConstants.LAST_DATE, DateTime.Now.Ticks);
            //        navigator.GoToMain();
            //}
            //View.HideLoading();

            navigator.GoToMain();
        }
    }
}

