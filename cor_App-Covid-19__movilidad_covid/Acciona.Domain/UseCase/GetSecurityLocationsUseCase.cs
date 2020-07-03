using Acciona.Domain.Model;
using Acciona.Domain.Model.Base;
using Acciona.Domain.Model.Employee;
using Acciona.Domain.Model.Security;
using Acciona.Domain.Repository;
using ServiceLocator;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Acciona.Domain.UseCase
{
    public class GetSecurityLocationsUseCase
    {
        private ISecurityRepository securityRepository;
        private IEnumerable<Location> cache;

        public GetSecurityLocationsUseCase()
        {
            securityRepository = Locator.Current.GetService<ISecurityRepository>();
        }

        public async Task<TaskGenericResponse<IEnumerable<Location>>> Execute()
        {            
            try
            {                
                var response = await securityRepository.GetLocations();
                cache = response;
                return new TaskGenericResponse<IEnumerable<Location>>() { Data = response };
            }
            catch (Exception e)
            {
                if (cache != null)
                    return new TaskGenericResponse<IEnumerable<Location>>() { Data = cache };
                ApiException errorException = e as ApiException;
                if (errorException != null)
                    return new TaskGenericResponse<IEnumerable<Location>>() { ErrorCode = errorException.Code, Message = errorException.Error };
                else
                    return new TaskGenericResponse<IEnumerable<Location>>() { ErrorCode = 600, Message = e.Message };
            }
        }
    }
}
