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
    public class GetPassportUseCase
    {
        private IEmployeeRepository employeeRepository;
        private ISettingsService settingsService;
        private Passport cachePassport;

        public GetPassportUseCase()
        {
            employeeRepository = Locator.Current.GetService<IEmployeeRepository>();
            settingsService = Locator.Current.GetService<ISettingsService>();
        }

        public async Task<TaskGenericResponse<Passport>> Execute(bool force=false)
        {
            if (!force && cachePassport != null)
                return new TaskGenericResponse<Passport>(){ Data = cachePassport};
            if (force)
                cachePassport = null;
            try
            {                
                var response = await employeeRepository.GetPassport();
                cachePassport = response;
                var serialized= JsonConvert.SerializeObject(response);
                settingsService.AddOrUpdateValue(DomainConstants.OFFLINE_PASSPORT, serialized);
                return new TaskGenericResponse<Passport>() { Data = response };
            }
            catch (Exception e)
            {
                ApiException errorException = e as ApiException;
                if (errorException != null)
                {
                    if (errorException.Code == 400)
                        return new TaskGenericResponse<Passport>() { Data = null };
                    return new TaskGenericResponse<Passport>() { ErrorCode = errorException.Code, Message = errorException.Error };
                }
                else
                    return new TaskGenericResponse<Passport>() { ErrorCode = 600, Message = e.Message };
            }
        }
    }
}
