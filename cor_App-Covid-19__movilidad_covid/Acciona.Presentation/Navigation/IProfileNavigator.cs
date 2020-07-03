using Acciona.Domain.Model.Employee;
using Presentation.Navigation.Base;


namespace Acciona.Presentation.Navigation
{
    public interface IProfileNavigator : IBaseNavigator
    {
        void GoToContactData(Ficha ficha);
        void GoToMedicalInfo(Ficha ficha);
        void GoToLogin();
        void GoLanguage();
        void GoToCenter(Ficha ficha);
    }
}
