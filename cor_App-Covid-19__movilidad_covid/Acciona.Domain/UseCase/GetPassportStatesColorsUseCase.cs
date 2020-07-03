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
    public class GetPassportStatesColorsUseCase
    {
        private IMasterRepository masterRepository;
        private IEnumerable<PassportStateColor> cache;

        public GetPassportStatesColorsUseCase()
        {
            masterRepository = Locator.Current.GetService<IMasterRepository>();
        }

        public async Task<TaskGenericResponse<IEnumerable<PassportStateColor>>> Execute(bool force=false)
        {
            if (!force && cache != null)
                return new TaskGenericResponse<IEnumerable<PassportStateColor>>(){ Data = cache};
            try
            {                
                var response = await masterRepository.GetPassportStatesColors();
                cache = response;
                return new TaskGenericResponse<IEnumerable<PassportStateColor>>() { Data = response };
            }
            catch (Exception e)
            {
                ApiException errorException = e as ApiException;
                if (errorException != null)
                {                  
                    return new TaskGenericResponse<IEnumerable<PassportStateColor>>() { ErrorCode = errorException.Code, Message = errorException.Error };
                }
                else
                    return new TaskGenericResponse<IEnumerable<PassportStateColor>>() { ErrorCode = 600, Message = e.Message };
            }
        }
    }
}
