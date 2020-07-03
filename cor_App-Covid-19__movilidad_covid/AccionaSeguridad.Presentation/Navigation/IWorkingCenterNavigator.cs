using System;
using Presentation.Navigation.Base;

namespace AccionaSeguridad.Presentation.Navigation
{
    public interface IWorkingCenterNavigator : IBaseNavigator
    {
        void GoBack();
    }
}
