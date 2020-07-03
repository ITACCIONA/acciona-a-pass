using Presentation.Navigation.Base;


namespace Acciona.Presentation.Navigation
{
    public interface IAlertsNavigator : IBaseNavigator
    {
        void GoBack();
        void GoToAlertDetail();
    }
}
