using System;
using Presentation.Navigation.Base;

namespace AccionaSeguridad.Presentation.Navigation
{
    public interface IConfigNavigator : IBaseNavigator
    {
        void GoBack();
        void GoLanguage();
        void GoCenter();
    }
}
