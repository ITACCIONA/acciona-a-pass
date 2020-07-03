using System;
using Presentation.Navigation.Base;

namespace AccionaSeguridad.Presentation.Navigation
{
    public interface ILanguageNavigator : IBaseNavigator
    {
        void GoBack();
        void RestarApp();
    }
}
