using System;
using System.Collections.Generic;
using System.Text;

namespace Presentation.UI.Base
{
    public interface IBaseErrorUI:IBaseUI
    {
        void HideError();
        void ShowError(String errorMessage, String actionText, String buttonText,Action action);
    }
}
