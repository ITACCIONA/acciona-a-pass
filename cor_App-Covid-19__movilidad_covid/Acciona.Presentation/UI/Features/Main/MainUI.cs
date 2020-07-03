using Acciona.Domain.Model;
using Acciona.Presentation.UI.Features.Splash;
using Presentation.UI.Base;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Acciona.Presentation.UI.Features.Main
{
    public interface MainUI : IBaseUI
    {
        void ShowState(MainPresenter.MainState state);
        void ShowNotificationsNotRead(bool notRead);
        void DownloadApp();
        int GetVersion();
        SplashPresenter.Platform GetPlatform();
    }
}
