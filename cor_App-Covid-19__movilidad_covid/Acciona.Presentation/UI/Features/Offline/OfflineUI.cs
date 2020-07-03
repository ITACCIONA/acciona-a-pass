using Acciona.Domain.Model.Employee;
using Presentation.UI.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Acciona.Presentation.UI.Features.Offline
{
    public interface OfflineUI : IBaseUI
    {
        void SetQRInfo(string v);
        void SetPassportInfo(Domain.Model.Employee.Passport passport, string message);
        string GetString(string v);
    }
}
