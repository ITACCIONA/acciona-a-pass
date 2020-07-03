using Acciona.Domain.Model.Employee;
using Presentation.UI.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Acciona.Presentation.UI.Features.AlertDetail
{
    public interface AlertDetailUI : IBaseUI
    {
        void SetAlert(Alert alert);
    }
}
