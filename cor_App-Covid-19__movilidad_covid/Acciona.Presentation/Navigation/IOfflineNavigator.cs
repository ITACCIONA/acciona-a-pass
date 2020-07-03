using Presentation.Navigation.Base;


namespace Acciona.Presentation.Navigation
{
    public interface IOfflineNavigator : IBaseNavigator
    {
        void GoToLogin();
        void GoToMain();
    }
}
