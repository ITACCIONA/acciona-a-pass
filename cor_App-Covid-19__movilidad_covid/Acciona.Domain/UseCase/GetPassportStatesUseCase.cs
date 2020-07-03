using Acciona.Domain.Model;
using Acciona.Domain.Model.Base;
using Acciona.Domain.Model.Master;
using Acciona.Domain.Repository;
using ServiceLocator;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Acciona.Domain.UseCase
{
    public class GetPassportStatesUseCase
    {
        private IMasterRepository masterRepository;
        private IEnumerable<PassportState> cache;

        public GetPassportStatesUseCase()
        {
            masterRepository = Locator.Current.GetService<IMasterRepository>();
        }

        public async Task<TaskGenericResponse<IEnumerable<PassportState>>> Execute(bool force=false)
        {
            if (!force && cache != null)
                return new TaskGenericResponse<IEnumerable<PassportState>>(){ Data = cache};
            try
            {                
                var response = await masterRepository.GetPassportStates();
                cache = response;
                return new TaskGenericResponse<IEnumerable<PassportState>>() { Data = response };
            }
            catch (Exception e)
            {
                ApiException errorException = e as ApiException;
                if (errorException != null)
                {                  
                    return new TaskGenericResponse<IEnumerable<PassportState>>() { ErrorCode = errorException.Code, Message = errorException.Error };
                }
                else
                    return new TaskGenericResponse<IEnumerable<PassportState>>() { ErrorCode = 600, Message = e.Message };
            }
        }
    }
}
