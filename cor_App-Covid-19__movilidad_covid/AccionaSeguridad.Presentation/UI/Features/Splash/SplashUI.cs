using Presentation.UI.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace AccionaSeguridad.Presentation.UI.Features.Splash
{
    public interface SplashUI : IBaseUI
    {
        int GetVersion();
        void Download();
    }
}
