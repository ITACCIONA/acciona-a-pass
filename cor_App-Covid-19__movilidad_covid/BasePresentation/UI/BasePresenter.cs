using Presentation.Navigation.Base;
using ServiceLocator;
using System;
using System.Collections.Generic;
using System.Text;

namespace Presentation.UI.Base
{
    public class BasePresenter<TUI, UNavigator> :IBasePresenterUI<TUI> where TUI : IBaseUI where UNavigator : IBaseNavigator
    {
        public TUI View { get; set; }
        public UNavigator navigator { get; set; }

        public BasePresenter()
        {            
            navigator = Locator.Current.GetService<UNavigator>();            
        }

        public virtual void OnCreate()
        {
            
        }

        public virtual void OnDestroy()
        {
            
        }

        public virtual void OnPause()
        {
            
        }

        public virtual void OnResume()
        {
            
        }
    }
}
