using Presentation.Navigation.Base;


namespace Acciona.Presentation.Navigation
{
    public interface ISplashNavigator : IBaseNavigator
    {
        void GoToLogin();
        void GoToMain();
        void GoToMedicalTest();
        void GoToOffline();
    }
}
