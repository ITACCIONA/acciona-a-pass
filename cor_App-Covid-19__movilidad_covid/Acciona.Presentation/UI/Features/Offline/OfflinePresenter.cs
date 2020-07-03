using Acciona.Domain;
using Acciona.Domain.Model;
using Acciona.Domain.Model.Employee;
using Acciona.Domain.UseCase;
using Acciona.Domain.Utils;
using Acciona.Presentation.Navigation;
using Domain.Services;
using Newtonsoft.Json;
using Presentation.UI.Base;
using ServiceLocator;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;

namespace Acciona.Presentation.UI.Features.Offline
{
    public class OfflinePresenter : BasePresenter<OfflineUI, IOfflineNavigator>
    {
        private readonly GetPassportUseCase getPassportUseCase;
        private readonly GetOfflinePassportUseCase getOfflinePassportUseCase;

        public OfflinePresenter()
        {
            getPassportUseCase = Locator.Current.GetService<GetPassportUseCase>();
            getOfflinePassportUseCase = Locator.Current.GetService<GetOfflinePassportUseCase>();
        }

        public override void OnCreate()
        {
            base.OnCreate();
            GetData();
        }

        public void GetData()
        {
            var passport = getOfflinePassportUseCase.Execute();
            View.SetQRInfo(QRUtils.GenerateQRInfo(passport));
            var message = CalculatePassportExpiration(passport);
            View.SetPassportInfo(passport, message);            
        }


        private string CalculatePassportExpiration(Domain.Model.Employee.Passport passport)
        {
            var now = DateTime.Now;
            if (passport.FechaExpiracion == null) return "";

            var dateCompare = DateTime.Compare(passport.FechaExpiracion.Value, now);


            // ReSharper disable once CommentTypo
            if (dateCompare < 0) //ha caducado, la fecha de expiración es anterior a la fecha actual
            {
                passport.ColorPasaporte = "Gris";
                return View.GetString("state_caducado");
            }

            if (dateCompare > 0) //todavia no ha caducado, vamos a calcular los dias que quedan para que caduque
            {
                var daysLeft = Math.Truncate((passport.FechaExpiracion.Value - now).TotalDays);
                var text = View.GetString("state_multiple_days").Replace("dd", daysLeft.ToString());
                CultureInfo info = new CultureInfo(Locator.Current.GetService<AppSession>().Language);
                if (daysLeft == 1)
                {
                    text = View.GetString("state_single_day").Replace("dd", passport.FechaExpiracion.Value.AddMinutes(-1).ToString("dd", info)).Replace("mm", passport.FechaExpiracion.Value.AddMinutes(-1).ToString("MMMM", info));
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
                    case "Gris":
                        return "";
                    default:
                        return text;
                }
            }
            else //la fecha es igual
            {

            }
            return "";
        }

        public async Task ContinueClick()
        {
            await Task.Delay(50);
            View.ShowLoading();
            var passportResponse = await getPassportUseCase.Execute(true);
            View.HideLoading();
            if (passportResponse.ErrorCode > 0)
            {
                if (passportResponse.ErrorCode == 401)
                    navigator.GoToLogin();
                else
                    View.ShowDialog("offline_error", "msg_ok", null);
            }
            else
            {
                navigator.GoToMain();
            }
        }
    }
}
