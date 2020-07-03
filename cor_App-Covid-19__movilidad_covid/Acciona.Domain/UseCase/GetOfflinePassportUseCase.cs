using Acciona.Domain.Model;
using Acciona.Domain.Model.Base;
using Acciona.Domain.Model.Employee;
using Acciona.Domain.Repository;
using Domain.Services;
using Newtonsoft.Json;
using ServiceLocator;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Acciona.Domain.UseCase
{
    public class GetOfflinePassportUseCase
    {
        private ISettingsService settingsService;
        
        public GetOfflinePassportUseCase()
        {
            settingsService = Locator.Current.GetService<ISettingsService>();
        }

        public Passport Execute()
        {
            try
            {
                string serialized = settingsService.GetValueOrDefault<string>(DomainConstants.OFFLINE_PASSPORT, null);
                if (serialized == null)
                    return null;
                return JsonConvert.DeserializeObject<Passport>(serialized);

            }
            catch(Exception e)
            {
                return null; //Compatibilidad con futuros cambios de pasaporte, se perderia el offline
            }
        }
    }
}
