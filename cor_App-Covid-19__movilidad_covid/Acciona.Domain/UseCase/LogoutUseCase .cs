using System;
using System.Threading.Tasks;
using Acciona.Domain.Model;
using Acciona.Domain.Model.Base;
using Acciona.Domain.Repository;
using Acciona.Domain.Services;
using Domain.Services;
using ServiceLocator;

namespace Acciona.Domain.UseCase
{
    public class LogoutUseCase
    {
        private const String LAST_SESSION = "LastSession";
        private const String LAST_DATE = "LastDate";

        private ISettingsService settingsService;

        public LogoutUseCase()
        {
            settingsService = Locator.Current.GetService<ISettingsService>();
        }

        public async Task Execute()
        {
            settingsService.AddOrUpdateValue<string>(LAST_SESSION, null);
            settingsService.AddOrUpdateValue<long>(LAST_DATE, DateTime.Now.AddDays(-1).Ticks);
            var appSession = Locator.Current.GetService<AppSession>();
            appSession.AccesToken = null;
        }
    }
}
