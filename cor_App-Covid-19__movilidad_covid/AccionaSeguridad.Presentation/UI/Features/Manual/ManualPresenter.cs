using Acciona.Domain;
using Acciona.Domain.Model;
using Acciona.Domain.UseCase;
using AccionaSeguridad.Presentation.Navigation;
using Domain.Services;
using Newtonsoft.Json;
using Presentation.UI.Base;
using ServiceLocator;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AccionaSeguridad.Presentation.UI.Features.Manual
{
    public class ManualPresenter : BasePresenter<ManualUI, IManualNavigator>
    {
        private readonly GetUserPaperUseCase getUserPaperUseCase;
        private GetSecurityPassportUseCase getSecurityPassportUseCase;

        public ManualPresenter()
        {
            getUserPaperUseCase = Locator.Current.GetService<GetUserPaperUseCase>();
            getSecurityPassportUseCase = Locator.Current.GetService<GetSecurityPassportUseCase>();
        }

        public override void OnCreate()
        {
            base.OnCreate();                           
        }

        public void BackClicked()
        {
            navigator.GoBack();
        }

      

        public async Task NextClicked(string document, string phone)
        {
            if (document.Trim().Length == 0)
            {
                View.ShowDialog("document_error", "msg_ok", null);
                return;
            }
            View.ShowLoading();
            var response = await getUserPaperUseCase.Execute(document, phone);
            if(response.ErrorCode>0)
            {
                View.HideLoading();
                View.ShowDialog(response.Message, "msg_ok", null);
            }
            else
            {
                var passportresponse = await getSecurityPassportUseCase.Execute(response.Data.IdEmpleado);
                if(passportresponse.ErrorCode>0)
                    View.ShowResult(response.Data);
                else if(passportresponse.Data==null)
                    View.ShowResult(response.Data);
                else 
                {
                    var info = new QRInfo()
                    {
                        IdEmpleado = passportresponse.Data.IdEmpleado,
                        FechaExpiracion = passportresponse.Data.FechaExpiracion,
                        PasaporteColor = passportresponse.Data.ColorPasaporte,
                        Manual = true
                    };
                    var now = DateTime.Now;
                    if (!info.FechaExpiracion.HasValue)
                    {
                        View.ShowResult(response.Data);
                    }
                    else
                    {
                        var dateCompare = DateTime.Compare(info.FechaExpiracion.Value, now);
                        if (dateCompare < 0) //ha caducado, la fecha de expiración es anterior a la fecha actual
                            View.ShowResult(response.Data);
                        else if (passportresponse.Data.ColorPasaporte == "Gris")
                        {
                            View.ShowResult(response.Data);
                        }
                        else
                        {
                            Locator.Current.GetService<AppSession>().QRInfo = info;
                            navigator.GoToResult();
                        }
                    }
                }
            }            
            View.HideLoading();
        }
    }
}
