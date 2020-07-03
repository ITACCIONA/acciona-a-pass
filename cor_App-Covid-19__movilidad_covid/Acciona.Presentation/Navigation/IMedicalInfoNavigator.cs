using Acciona.Domain.Model.Employee;
using Presentation.Navigation.Base;


namespace Acciona.Presentation.Navigation
{
    public interface IMedicalInfoNavigator : IBaseNavigator
    {
        void GoBack();
        void GoToForm();
        void GoMedicalInfoEdit(Ficha ficha);
    }
}
