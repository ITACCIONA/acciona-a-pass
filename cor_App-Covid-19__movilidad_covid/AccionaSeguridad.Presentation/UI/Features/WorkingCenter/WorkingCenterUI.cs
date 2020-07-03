using System;
using System.Collections.Generic;
using Acciona.Domain.Model.Employee;
using Acciona.Domain.Model.Security;
using Presentation.UI.Base;

namespace Acciona.Presentation.UI.Features.WorkingCenter
{
    public interface WorkingCenterUI : IBaseUI
    {
        void SetSelectedLocation(Location location);
        void SetLocations(IEnumerable<Location> locations);
    }
}
