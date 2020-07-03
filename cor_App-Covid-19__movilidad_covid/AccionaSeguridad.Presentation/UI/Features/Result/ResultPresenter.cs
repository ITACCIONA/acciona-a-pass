using Acciona.Domain;
using Acciona.Domain.Model;
using Acciona.Domain.UseCase;
using AccionaSeguridad.Presentation.Navigation;
using Domain.Services;
using Microsoft.AppCenter.Analytics;
using Newtonsoft.Json;
using Presentation.UI.Base;
using ServiceLocator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AccionaSeguridad.Presentation.UI.Features.Result
{
    public class ResultPresenter : BasePresenter<ResultUI, IResultNavigator>
    {
        private QRInfo info;
        private bool online;
        private bool temperaturRensponded;
        private bool? temperatureValue;

        private SaveTemperatureUseCase saveTemperatureUseCase;
        private GetSecurityPassportUseCase getSecurityPassportUseCase;
        private Dictionary<string, string> dict;

        public ResultPresenter()
        {
            saveTemperatureUseCase = Locator.Current.GetService<SaveTemperatureUseCase>();
            getSecurityPassportUseCase = Locator.Current.GetService<GetSecurityPassportUseCase>();
        }

        public override void OnCreate()
        {
            base.OnCreate();
            GetData();
        }

        private async Task GetData()
        {
            info = Locator.Current.GetService<AppSession>().QRInfo;
            online = true;
            View.ShowLoading();
            var response = await getSecurityPassportUseCase.Execute(info.IdEmpleado);
            View.HideLoading();
            dict = new Dictionary<string, string>();
            dict["manual"] = info.Manual.ToString();
            if (response.ErrorCode > 0)
            {
                online = false;
                if(info.Manual)
                {
                    View.ShowDialog(response.Message, "msg_ok", () => navigator.GoBack());
                    return;
                }                                
                dict["online"] = "false";                
            }
            else
            {
                info=new QRInfo()
                {
                    IdEmpleado=response.Data.IdEmpleado,
                    FechaExpiracion=response.Data.FechaExpiracion,
                    PasaporteColor=response.Data.ColorPasaporte
                };
                dict["online"] = "true";                
            }
            var now = DateTime.Now;
            if (!info.FechaExpiracion.HasValue)
            {
                info.PasaporteColor = "Gris";
            }
            else
            {
                var dateCompare = DateTime.Compare(info.FechaExpiracion.Value, now);
                if (dateCompare < 0) //ha caducado, la fecha de expiración es anterior a la fecha actual
                    info.PasaporteColor = "Gris";
            }
            dict["color"] = info.PasaporteColor;
            string hashCode = SHA1(info.IdEmpleado.ToString());
            dict["empleado"] = hashCode;
            var loc = Locator.Current.GetService<AppSession>().Location;
            if (loc != null)
            {
                dict["pais"] = loc.Pais;
                dict["ciudad"] = loc.Ciudad;
                dict["centro"] = loc.Name;
                dict["centroId"] = loc.IdLocation.ToString();
                dict["centroExtra"] = loc.Extra!=null?loc.Extra:"";
            }
            if (info.PasaporteColor.Equals("Verde"))
                View.AskTemperature();
            else
            {
                dict["temperatura"] = "NA";
                Analytics.TrackEvent("SeguridadPasaporte", dict);
                View.SetInfo(info, online);
                SendInfo();
            }
        }

        private string SHA1(string text)
        {
            var result = default(string);

            using (var algo = new SHA1Managed())
            {
                result = GenerateHashString(algo, text);
            }

            return result;
        }

        private string GenerateHashString(HashAlgorithm algo, string text)
        {
            // Compute hash from text parameter
            algo.ComputeHash(Encoding.UTF8.GetBytes(text));

            // Get has value in array of bytes
            var result = algo.Hash;

            // Return as hexadecimal string
            return string.Join(
                string.Empty,
                result.Select(x => x.ToString("x2")));
        }

        private void SendInfo()
        {
            if (temperaturRensponded && temperatureValue.HasValue)
            {
                saveTemperatureUseCase.Execute(new TemperatureSincro()
                {
                    IdDevice="-",
                    IdEmpleado=info.IdEmpleado,
                    IsTemperatureOver=temperatureValue.Value,
                    Date=DateTime.Now.ToUniversalTime().Ticks //Al guardar en sqlite perdemos offset
                });
            }            
        }

        public void ScreenTouched()
        {
            navigator.GoBack();
        }

        public void TemperatureAskResponded(bool? response)
        {
            temperaturRensponded = true;
            temperatureValue = response;            
            if (response.HasValue && response.Value) {
                dict["color"] = "Rojo";
                info.PasaporteColor = "Rojo";                
            }
            dict["temperatura"] = response.HasValue ? response.Value.ToString() : "-";
            Analytics.TrackEvent("SeguridadPasaporte", dict);
            View.SetInfo(info, online);
            SendInfo();

        }
    }
}
