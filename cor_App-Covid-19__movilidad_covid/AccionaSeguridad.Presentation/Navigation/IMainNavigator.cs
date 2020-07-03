using Presentation.Navigation.Base;

namespace AccionaSeguridad.Presentation.Navigation
{
    public interface IMainNavigator : IBaseNavigator
    {
        void GoToLogin();
        void GoResult();
        void GoManual();
        void GoConfig();
        void GoSplash();
        void OpenCenter();
    }
}
