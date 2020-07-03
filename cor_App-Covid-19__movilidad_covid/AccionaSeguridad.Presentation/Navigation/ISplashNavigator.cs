using Presentation.Navigation.Base;


namespace AccionaSeguridad.Presentation.Navigation
{
    public interface ISplashNavigator : IBaseNavigator
    {
        void GoToLogin();
        void GoToMain();
    }
}
