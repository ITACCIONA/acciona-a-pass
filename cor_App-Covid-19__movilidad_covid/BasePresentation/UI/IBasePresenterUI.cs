using System;
using System.Collections.Generic;
using System.Text;

namespace Presentation.UI.Base
{
    public interface IBasePresenterUI<T>:IBasePresenter where T:IBaseUI  
    {
        T View { get; set; }
    }
}
