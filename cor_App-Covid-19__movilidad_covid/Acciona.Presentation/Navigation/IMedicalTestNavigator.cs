using Presentation.Navigation.Base;


namespace Acciona.Presentation.Navigation
{
    public interface IMedicalTestNavigator : IBaseNavigator
    {        
        void GoToMain();
        void GoBack();
    }
}
