using Presentation.UI.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Acciona.Presentation.UI.Features.Health
{
    public interface HealthUI : IBaseUI
    {
        void ShowFirstStep();
        void ShowSecondStep();
        string GetString(string v);
    }
}
