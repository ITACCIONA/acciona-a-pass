using System;
using Acciona.Domain.Model.Employee;
using Presentation.UI.Base;

namespace Acciona.Presentation.UI.Features.Language
{
    public interface LanguageUI : IBaseUI
    {
        void SetConfiguredLang(string configuredLang);
    }
}
