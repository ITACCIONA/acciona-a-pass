using Presentation.Navigation.Base;


namespace AccionaSeguridad.Presentation.Navigation
{
    public interface IManualNavigator : IBaseNavigator
    {
        void GoBack();
        void GoToResult();        
    }
}
