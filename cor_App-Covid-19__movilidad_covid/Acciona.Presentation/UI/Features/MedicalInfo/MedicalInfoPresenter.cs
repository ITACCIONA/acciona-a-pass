using Acciona.Domain;
using Acciona.Domain.Model;
using Acciona.Domain.Model.Employee;
using Acciona.Presentation.Navigation;
using Domain.Services;
using Newtonsoft.Json;
using Presentation.UI.Base;
using ServiceLocator;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Acciona.Presentation.UI.Features.MedicalInfo
{
    public class MedicalInfoPresenter : BasePresenter<MedicalInfoUI, IMedicalInfoNavigator>
    {
        
        public Ficha ficha;
        private bool?[] responses = new bool?[3];

        public override void OnCreate()
        {
            base.OnCreate();
            responses[0] = responses[1] = responses[2] = null;

            getResponsesFromFicha();
            View.setResponsesValues(responses);
        }

        public enum Steps
        {
            Step1, Step2
        }
        
        private Steps actualStep;

        public void BackClicked()
        {
            switch (actualStep)
            {
                case Steps.Step1:
                    navigator.GoBack();
                    break;
                case Steps.Step2:
                    actualStep = Steps.Step2;
                    View.ShowMedicalInfoStep();
                    break;
            }
        }
        
        public void QuestionFirstStepClicked(bool? value)
        {
            responses[0] = value;
            actualStep = Steps.Step2;
            View.ShowMedicalInfoStep();
            View.setResponsesValues(responses);
        }      

        public void ModifyRiskClicked()
        {
            //actualStep = Steps.Step1;
            //View.showRiskModification();
            navigator.GoMedicalInfoEdit(ficha);
        }

        public void ModifyDataClicked()
        {
            navigator.GoToForm();
        }

        private void getResponsesFromFicha()
        {
            foreach (var riskFactor in ficha.ValoracionFactorRiesgos)
            {
                if (riskFactor.Name != "Vulnerables") continue;
                responses[0] = riskFactor.Value;
                break;
            }
            foreach (var riskFactor in ficha.ValoracionFactorRiesgos)
            {
                if (riskFactor.Name != "Positivo") continue;
                responses[1] = riskFactor.Value;
                break;
            }
            foreach (var riskFactor in ficha.ValoracionFactorRiesgos)
            {
                if (riskFactor.Name != "AltaExposicion") continue;
                responses[2] = riskFactor.Value;
                break;
            }
        }
    }
}
