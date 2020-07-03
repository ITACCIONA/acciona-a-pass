using Presentation.Navigation.Base;


namespace Acciona.Presentation.Navigation
{
    public interface ILoginNavigator : IBaseNavigator
    {
        void GoToMain();
        void GoToRegistered();
        void GoToMedicalTest();
    }
}
