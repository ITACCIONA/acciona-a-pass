using Presentation.UI.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace AccionaSeguridad.Presentation.UI.Features.Manual
{
    public interface ManualUI : IBaseUI
    {
        void ShowResult(Acciona.Domain.Model.Security.UserPaper data);
    }
}
