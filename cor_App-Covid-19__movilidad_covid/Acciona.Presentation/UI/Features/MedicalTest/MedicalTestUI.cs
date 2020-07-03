using Presentation.UI.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Acciona.Presentation.UI.Features.MedicalTest
{
    public interface MedicalTestUI : IBaseUI
    {
        void ShowFirstStep();
        void ShowSecondStep();
        void ShowThirdStep();
        void ShowConclusionStep(bool?[] responses);
        void ConfigureTitleAndBack(bool canBack);
    }
}
