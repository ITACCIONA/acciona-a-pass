using Acciona.Domain.Model;
using Acciona.Domain.Model.Security;
using Presentation.UI.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AccionaSeguridad.Presentation.UI.Features.Main
{
    public interface MainUI : IBaseUI
    {
        void ShowQRScanner();
        void SetSelectedLocation(Location location);
    }
}
