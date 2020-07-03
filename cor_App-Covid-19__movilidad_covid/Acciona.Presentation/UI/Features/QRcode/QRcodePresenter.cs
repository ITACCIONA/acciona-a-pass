using Acciona.Domain;
using Acciona.Domain.Model;
using Acciona.Domain.UseCase;
using Acciona.Domain.Utils;
using Acciona.Presentation.Navigation;
using Domain.Services;
using Newtonsoft.Json;
using Presentation.UI.Base;
using ServiceLocator;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace Acciona.Presentation.UI.Features.QRcode
{
    public class QRcodePresenter : BasePresenter<QRcodeUI, IQRcodeNavigator>
    {
        private readonly GetPassportUseCase getPassportUseCase;
        private readonly GetOfflinePassportUseCase getOfflinePassportUseCase;
        private readonly GetResourceURLUseCase getResourceURLUseCase;
        private readonly ISettingsService settingsService;
        private int estadoPasaporte;

        public QRcodePresenter()
        {
            getPassportUseCase = Locator.Current.GetService<GetPassportUseCase>();
            getOfflinePassportUseCase = Locator.Current.GetService<GetOfflinePassportUseCase>();
            getResourceURLUseCase = Locator.Current.GetService<GetResourceURLUseCase>();
            settingsService = Locator.Current.GetService<ISettingsService>();
        }


        public override void OnCreate()
        {
            base.OnCreate();
            GetData();
        }

        public async Task GetData()
        {
            await Task.Delay(50);
            View.ShowLoading();
            var passport = getOfflinePassportUseCase.Execute();
            bool offline = false;
            var passportResponse = await getPassportUseCase.Execute(true);
            View.HideLoading();
            if (passportResponse.ErrorCode > 0)
            {
                if (passport == null)
                {
                    View.ShowDialog(passportResponse.Message, "msg_ok", null);
                    return;
                }
                offline = true;
            }
            else
            {
                passport = passportResponse.Data;
            }
            View.SetQRInfo(QRUtils.GenerateQRInfo(passport));
            var message = CalculatePassportExpiration(passport);
            View.SetPassportInfo(passport, message, offline);

            estadoPasaporte = passport.EstadoId;
            int state = settingsService.GetValueOrDefault<int>(DomainConstants.LAST_PASSPORT_STATE, -1);
            if (state != estadoPasaporte)
            {
                if (passport.HasMessage)
                    View.ShowStateChange();
                settingsService.AddOrUpdateValue<int>(DomainConstants.LAST_PASSPORT_STATE, estadoPasaporte);
            }
        }

        

        public void ToDoClicked()
        {
            View.ShowTodo(getResourceURLUseCase.Execute(estadoPasaporte));
        }

        private string CalculatePassportExpiration(Domain.Model.Employee.Passport passport)
        {
            var now = DateTime.Now;
            if (passport.FechaExpiracion == null) return "";
            
            var dateCompare = DateTime.Compare(passport.FechaExpiracion.Value, now);
            
               
            // ReSharper disable once CommentTypo
            if (dateCompare < 0) //ha caducado, la fecha de expiración es anterior a la fecha actual
            {
                View.SetCaducado(true);
                return View.GetString("state_caducado");
            }

            if(dateCompare > 0) //todavia no ha caducado, vamos a calcular los dias que quedan para que caduque
            {
                var daysLeft = Math.Truncate((passport.FechaExpiracion.Value - now).TotalDays);
                var text =View.GetString("state_multiple_days").Replace("dd",daysLeft.ToString());
                CultureInfo info = new CultureInfo(Locator.Current.GetService<AppSession>().Language);
                if (daysLeft == 1)
                {
                    text = View.GetString("state_single_day").Replace("dd",passport.FechaExpiracion.Value.AddMinutes(-1).ToString("dd",info)).Replace("mm", passport.FechaExpiracion.Value.AddMinutes(-1).ToString("MMMM",info));
                }
                else if (daysLeft < 1)
                {
                    text = View.GetString("state_this_day").Replace("dd", passport.FechaExpiracion.Value.AddMinutes(-1).ToString("dd", info)).Replace("mm", passport.FechaExpiracion.Value.AddMinutes(-1).ToString("MMMM", info));
                    /*var hours = (passport.FechaExpiracion.Value - now).Hours;
                    if (hours == 1)
                    {
                        lasString = " hora</b>";
                        daysLeft = hours;
                    }
                    else if (hours > 0)
                    {
                        lasString = " horas</b>";
                        daysLeft = hours;
                    }                
                    else
                    {
                        var minutes = (passport.FechaExpiracion.Value - now).Minutes;
                        if (minutes == 1)
                        {
                            lasString = " minuto</b>";
                            daysLeft = minutes;
                        }
                        else
                        {
                            lasString = " minutos</b>";
                            daysLeft = minutes;
                        }
                    }*/
                }

                switch (passport.ColorPasaporte)
                {                                    
                    case "Gris" :
                        View.SetCaducado(false);
                        return "";
                    default:
                        View.SetCaducado(false);
                        return text;

                }
            }
            else //la fecha es igual
            {
                    
            }
            View.SetCaducado(false);
            return "";

        }

        public void RenewClick()
        {
            Locator.Current.GetService<AppSession>().Panic = false;
            navigator.GotToHealth();
        }

    }
}
