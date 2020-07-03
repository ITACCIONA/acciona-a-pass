using Presentation.UI.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Acciona.Presentation.UI.Features.QRcode
{
    public interface QRcodeUI : IBaseUI
    {
        void SetQRInfo(string info);
        void SetCaducado(bool v);
        void SetPassportInfo(Domain.Model.Employee.Passport passport, string message, bool offline);
        void ShowTodo(string url);
        void ShowStateChange();
        string GetString(string v);
    }
}
