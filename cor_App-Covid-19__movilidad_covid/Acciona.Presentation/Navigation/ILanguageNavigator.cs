using System;
using Presentation.Navigation.Base;

namespace Acciona.Presentation.Navigation
{
    public interface ILanguageNavigator : IBaseNavigator
    {
        void GoBack();
        void RestarApp();
    }
}
