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
    public class GetLocationsUseCase
    {
        private IMasterRepository masterRepository;
        private IEnumerable<Location> cache;


        public GetLocationsUseCase()
        {
            masterRepository = Locator.Current.GetService<IMasterRepository>();
        }

        public async Task<TaskGenericResponse<IEnumerable<Location>>> Execute(bool force=false)
        {
            if (!force && cache != null)
                return new TaskGenericResponse<IEnumerable<Location>>() { Data = cache };
            try
            {                
                var response = await masterRepository.GetLocations();
                cache = response;
                return new TaskGenericResponse<IEnumerable<Location>>() { Data = response };
            }
            catch (Exception e)
            {
                ApiException errorException = e as ApiException;
                if (errorException != null)
                    return new TaskGenericResponse<IEnumerable<Location>>() { ErrorCode = errorException.Code, Message = errorException.Error };
                else
                    return new TaskGenericResponse<IEnumerable<Location>>() { ErrorCode = 600, Message = e.Message };
            }
        }
    }
}
