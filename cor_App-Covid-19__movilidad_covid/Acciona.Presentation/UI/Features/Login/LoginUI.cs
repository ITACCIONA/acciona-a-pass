using Acciona.Domain.Model;
using Presentation.UI.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Acciona.Presentation.UI.Features.Login
{
    public interface LoginUI : IBaseUI
    {
        void SetLastUser(string oldUser);
        bool CheckInternet();

        void showLoadingLogin();

        void ShowErrorLogin();
        void ShowRetry();
    }
}
