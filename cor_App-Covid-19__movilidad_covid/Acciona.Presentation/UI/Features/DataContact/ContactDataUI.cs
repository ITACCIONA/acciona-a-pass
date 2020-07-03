using System;
using Acciona.Domain.Model.Employee;
using Presentation.UI.Base;

namespace Acciona.Presentation.UI.Features.DataContact
{
    public interface ContactDataUI : IBaseUI
    {
        void setProfileData(Ficha ficha);
    }
}
