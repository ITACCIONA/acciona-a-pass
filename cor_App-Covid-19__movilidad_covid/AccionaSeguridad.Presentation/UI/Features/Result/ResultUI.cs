using Acciona.Domain.Model;
using Presentation.UI.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace AccionaSeguridad.Presentation.UI.Features.Result
{
    public interface ResultUI : IBaseUI
    {
        void SetInfo(QRInfo info, bool online);
        void AskTemperature();
    }
}
