using Acciona.Domain;
using Acciona.Domain.Model;
using Acciona.Domain.Model.Employee;
using Acciona.Domain.Model.Master;
using Acciona.Domain.Services;
using Acciona.Domain.UseCase;
using Acciona.Presentation.Navigation;
using Domain.Services;
using Microsoft.AppCenter.Analytics;
using Newtonsoft.Json;
using Presentation.UI.Base;
using ServiceLocator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acciona.Presentation.UI.Features.Health
{
    public class HealthPresenter : BasePresenter<HealthUI, IHealthNavigator>
    {

        public enum HealthSteps
        {
            Step1,Step2
        }

        private readonly GetSymptomTypesUseCase getSymptomsTypesUseCase;
        private readonly SendSymptomsUseCase sendSymptomsUseCase;
        private readonly GetPassportUseCase getPassportUseCase;
        private IEnumerable<SymptomType> symptoms;
        private HealthSteps actualStep;
        private bool panic;

        private bool?[] responses;
        private string[] keys;
        private string[] texts;

        public HealthPresenter()
        {
            getSymptomsTypesUseCase = Locator.Current.GetService<GetSymptomTypesUseCase>();
            sendSymptomsUseCase = Locator.Current.GetService<SendSymptomsUseCase>();
            getPassportUseCase = Locator.Current.GetService<GetPassportUseCase>();            
        }

        public override void OnCreate()
        {
            base.OnCreate();
            responses = new bool?[3];
            keys = new string[3] { "Fiebre", "OtrosSintomas", "Contacto" };
            texts = new string[3] { "health_fiebre_confirm", "health_sintomas_confirm", "health_contacto_confirm" };
            GetData();
        }

        private async Task GetData()
        {
            await Task.Delay(100); //Pantalla este construida
            panic = Locator.Current.GetService<AppSession>().Panic;
            View.ShowLoading();
            var response = await getSymptomsTypesUseCase.Execute();
            View.HideLoading();
            if (response.ErrorCode > 0)
            {
                if (response.ErrorCode == 401)
                    View.ShowDialog(response.Message, "msg_ok", () => Locator.Current.GetService<ILogoutService>().LogoutExpired());
                else
                    View.ShowDialog(response.Message, "msg_ok", null);
            }
            else
            {
                symptoms = response.Data;
                actualStep = HealthSteps.Step1;
                View.ShowFirstStep();
            }
            
        }

        public void BackClicked()
        {
            switch (actualStep)
            {
                case HealthSteps.Step1:
                    navigator.GoBack();
                    break;
                case HealthSteps.Step2:
                    actualStep = HealthSteps.Step1;
                    View.ShowFirstStep();
                    break;
            }
        }

        public void BackSeconStepClicked()
        {
            actualStep = HealthSteps.Step1;
            View.ShowFirstStep();
        }

        public void ButtonNoSymptomClicked(bool? fiebre,bool? sintomas)
        {
            responses[0] = fiebre;
            responses[1] = sintomas;
            actualStep = HealthSteps.Step2;
            View.ShowSecondStep();
        }


        public async void ButtonMeetClicked(bool meet)
        {
            responses[2] = meet;
            View.ShowLoading();            
            List<RequestValue> values = new List<RequestValue>();
            string message = View.GetString("health_ask_confirm");
            bool sintomas = false;
            for (int i = 0; i < responses.Length; i++)
            {
                var value = new RequestValue()
                {
                    Id = symptoms.First(x => x.Name.Equals(keys[i])).IdSymptomTypes,
                    Value = responses[i]                    
                };
                values.Add(value);
                if (responses[i].HasValue && responses[i].Value)
                {
                    sintomas = true;
                    message = message + "\n" + View.GetString(texts[i]);
                }
            }
            if (sintomas)
            {
                View.ShowDialog(message, "msg_cancel",()=> View.HideLoading(), "health_yes_confirm", async () => await SenSimtomps(values, sintomas));
            }
            else
            {
                await SenSimtomps(values, sintomas);
            }
        }

        private async Task SenSimtomps(List<RequestValue> values, bool sintomas)
        {

            Dictionary<string, string> dict = new Dictionary<string, string>();
            for (int i = 0; i < responses.Length; i++)
            {
                dict[keys[i]] = responses[i].ToString();
            }
            dict["MeSientomal"] = panic.ToString();
            Analytics.TrackEvent("RenovarPasaporte",dict);
            var response = await sendSymptomsUseCase.Execute(values, panic && sintomas);
            if (response.ErrorCode > 0)
            {
                View.HideLoading();
                if (response.ErrorCode == 401)
                    View.ShowDialog(response.Message, "msg_ok", () => Locator.Current.GetService<ILogoutService>().LogoutExpired());
                else
                    View.ShowDialog(response.Message, "msg_ok", null);
            }
            else
            {
                await getPassportUseCase.Execute(true);
                View.HideLoading();
                navigator.GoBack();
            }
        }
    }
}
