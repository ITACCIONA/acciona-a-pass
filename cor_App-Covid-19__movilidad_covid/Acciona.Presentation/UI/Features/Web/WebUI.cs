using Presentation.UI.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Acciona.Presentation.UI.Features.Web
{
    public interface WebUI : IBaseUI
    {
        void Close();
        void OpenUrl(string url);
        void OnError();
        void OnOk(IDictionary<string, string> values);
    }
}
