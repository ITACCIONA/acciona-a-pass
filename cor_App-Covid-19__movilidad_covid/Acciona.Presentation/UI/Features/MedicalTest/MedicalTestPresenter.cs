using Acciona.Domain;
using Acciona.Domain.Model;
using Acciona.Domain.Model.Employee;
using Acciona.Domain.Model.Master;
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

namespace Acciona.Presentation.UI.Features.MedicalTest
{
    public class MedicalTestPresenter : BasePresenter<MedicalTestUI, IMedicalTestNavigator>
    {
        public enum Steps
        {
            Step1,Step2,Step3,Step4
        }

        private readonly GetRiskFactorsUseCase getRiskFactorsUseCase;
        private readonly SendRiskFactorsUseCase senrRiskFactorsUseCase;
        //private readonly GetPassportUseCase getPassportUseCase;

        private bool?[] responses;
        private string[] keys;
        private IEnumerable<RiskFactor> factors;
        private Steps actualStep;
        public bool canBack;

        public MedicalTestPresenter()
        {
            getRiskFactorsUseCase = Locator.Current.GetService<GetRiskFactorsUseCase>();
            senrRiskFactorsUseCase = Locator.Current.GetService<SendRiskFactorsUseCase>();
            //getPassportUseCase = Locator.Current.GetService<GetPassportUseCase>();
        }

        public override void OnCreate()
        {
            base.OnCreate();
            responses = new bool?[3];
            keys = new string[3] { "Vulnerables", "Positivo", "AltaExposicion" };
            View.ConfigureTitleAndBack(canBack);
            GetData();
        }

        private async Task GetData()
        {            
            await Task.Delay(100); //Pantalla este construida
            View.ShowLoading();
            var response=await getRiskFactorsUseCase.Execute();
            View.HideLoading();
            if (response.ErrorCode > 0)
            {
                View.ShowDialog(response.Message, "msg_ok", null);
            }
            else
            {
                factors = response.Data;
                actualStep = Steps.Step1;
                View.ShowFirstStep();
            }
        }

        public void BackClicked()
        {
            switch (actualStep)
            {
                case Steps.Step1:
                    if(canBack)
                        navigator.GoBack();
                    break;
                case Steps.Step2:
                    actualStep = Steps.Step1;
                    View.ShowFirstStep();
                    break;
                case Steps.Step3:
                    actualStep = Steps.Step2;
                    View.ShowSecondStep();
                    break;
                case Steps.Step4:
                    actualStep = Steps.Step3;
                    View.ShowThirdStep();
                    break;
            }
        }

        public async Task TestFinished()
        {
            View.ShowLoading();
            List<RequestValue> values = new List<RequestValue>();
            for(int i = 0; i < responses.Length; i++)
            {
                var value = new RequestValue()
                {
                    Id = factors.First(x => x.Name.Equals(keys[i])).IdRiskFactor,
                    Value = responses[i]
                };
                values.Add(value);
            }
            var response = await senrRiskFactorsUseCase.Execute(values);            
            if (response.ErrorCode > 0)
            {
                View.HideLoading();
                View.ShowDialog(response.Message, "msg_ok", null);
            }
            else
            {
                //await getPassportUseCase.Execute(true);
                View.HideLoading();
                navigator.GoToMain();
            }            
        }


        public void QuestionThirdStepClicked(bool? value)
        {
            responses[2] = value;
            actualStep = Steps.Step4;
            View.ShowConclusionStep(responses);
        }
        

        public void QuestionSecondStepClicked(bool? value)
        {
            responses[1] = value;
            actualStep = Steps.Step3;
            View.ShowThirdStep();
        }


        public void QuestionFirstStepClicked(bool? value)
        {
            responses[0] = value;
            actualStep = Steps.Step2;
            View.ShowSecondStep();
        }        
    }
}
