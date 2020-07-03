using System;
using System.Collections.Generic;
using System.Text;

namespace Presentation.UI.Base
{
    public interface IBasePresenter
    {
        void OnCreate();
        void OnResume();
        void OnPause();
        void OnDestroy();
        //bool BackClicked();
    }
}
