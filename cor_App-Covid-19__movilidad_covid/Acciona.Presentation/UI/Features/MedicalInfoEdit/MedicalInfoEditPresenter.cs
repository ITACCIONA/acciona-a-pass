using Acciona.Domain;
using Acciona.Domain.Model;
using Acciona.Domain.Model.Employee;
using Acciona.Domain.UseCase;
using Acciona.Presentation.Navigation;
using Domain.Services;
using Newtonsoft.Json;
using Presentation.UI.Base;
using ServiceLocator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acciona.Presentation.UI.Features.MedicalInfoEdit
{
    public class MedicalInfoEditPresenter : BasePresenter<MedicalInfoEditUI, IMedicalInfoEditNavigator>
    {
        private readonly GetRiskFactorsUseCase getRiskFactorsUseCase;
        private readonly SendRiskFactorsUseCase senrRiskFactorsUseCase;

        public Ficha Ficha { get; set; }
        private bool?[] responses = new bool?[3];
        private string[] keys;

        public MedicalInfoEditPresenter()
        {
            getRiskFactorsUseCase = Locator.Current.GetService<GetRiskFactorsUseCase>();
            senrRiskFactorsUseCase = Locator.Current.GetService<SendRiskFactorsUseCase>();
        }

        public override void OnCreate()
        {
            base.OnCreate();
            responses[0] = responses[1] = responses[2] = null;
            keys = new string[3] { "Vulnerables", "Positivo", "AltaExposicion" };
            GetResponsesFromFicha();
        }

        private void GetResponsesFromFicha()
        {
            foreach (var riskFactor in Ficha.ValoracionFactorRiesgos)
            {
                if (riskFactor.Name != keys[0]) continue;
                responses[0] = riskFactor.Value;
                break;
            }
            foreach (var riskFactor in Ficha.ValoracionFactorRiesgos)
            {
                if (riskFactor.Name != keys[1]) continue;
                responses[1] = riskFactor.Value;
                break;
            }
            foreach (var riskFactor in Ficha.ValoracionFactorRiesgos)
            {
                if (riskFactor.Name != keys[2]) continue;
                responses[2] = riskFactor.Value;
                break;
            }
        }

        public void BackClicked()
        {
            navigator.GoBack();
        }

        public async Task QuestionFirstStepClicked(bool? v)
        {
            responses[0] = v;
            View.ShowLoading();
            var response = await getRiskFactorsUseCase.Execute();
            View.HideLoading();
            if (response.ErrorCode > 0)
            {
                View.ShowDialog(response.Message, "msg_ok", null);
            }
            else
            {
                var factors = response.Data;
                List<RequestValue> values = new List<RequestValue>();
                for (int i = 0; i < responses.Length; i++)
                {
                    var value = new RequestValue()
                    {
                        Id = factors.First(x => x.Name.Equals(keys[i])).IdRiskFactor,
                        Value = responses[i]
                    };
                    values.Add(value);
                }
                var responseRisk = await senrRiskFactorsUseCase.Execute(values);
                if (responseRisk.ErrorCode > 0)
                {
                    View.HideLoading();
                    View.ShowDialog(responseRisk.Message, "msg_ok", null);
                }
                else
                {                    
                    View.HideLoading();
                    navigator.GoToMain();
                }
            }
        }

         
    }
}
