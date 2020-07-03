using Presentation.UI.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace AccionaSeguridad.Presentation.UI.Features.ManualResult
{
    public interface ManualResultUI : IBaseUI
    {
        void Close();
        void SetName(string v);
    }
}
