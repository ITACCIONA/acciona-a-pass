using Presentation.UI.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Acciona.Presentation.UI.Features.MedicalInfo
{
    public interface MedicalInfoUI : IBaseUI
    {
        void showRiskModification();
        void ShowMedicalInfoStep();
        void setResponsesValues(bool? [] responses);
    }
}
