using Acciona.Domain.Model.Employee;
using Presentation.UI.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Acciona.Presentation.UI.Features.Profile
{
    public interface ProfileUI : IBaseUI
    {
        void SetFicha(Ficha ficha);
    }
}
