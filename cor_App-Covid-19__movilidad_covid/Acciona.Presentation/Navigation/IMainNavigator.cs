using Presentation.Navigation.Base;

namespace Acciona.Presentation.Navigation
{
    public interface IMainNavigator : IBaseNavigator
    {
        void GoToAlarm();
        void GoToAlerts();
        void GotToHealth();
    }
}
