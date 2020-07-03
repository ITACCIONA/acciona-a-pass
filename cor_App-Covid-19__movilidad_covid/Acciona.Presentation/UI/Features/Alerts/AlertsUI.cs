using Acciona.Domain.Model.Employee;
using Presentation.UI.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Acciona.Presentation.UI.Features.Alerts
{
    public interface AlertsUI : IBaseUI
    {
        void SetAlerts(IEnumerable<Alert> alerts);
    }
}
