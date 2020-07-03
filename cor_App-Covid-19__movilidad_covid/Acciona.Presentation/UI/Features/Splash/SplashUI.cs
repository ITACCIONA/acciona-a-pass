using Presentation.UI.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Acciona.Presentation.UI.Features.Splash
{
    public interface SplashUI : IBaseUI
    {
        SplashPresenter.Platform GetPlatform();
        int GetVersion();
        void DownloadApp();
    }
}
