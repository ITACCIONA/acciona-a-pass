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
    public class GetMedicalMonitorsUseCase
    {
        private IMasterRepository masterRepository;
        private IEnumerable<MedicalMonitor> cache;

        public GetMedicalMonitorsUseCase()
        {
            masterRepository = Locator.Current.GetService<IMasterRepository>();
        }

        public async Task<TaskGenericResponse<IEnumerable<MedicalMonitor>>> Execute(bool force=false)
        {
            if (!force && cache != null)
                return new TaskGenericResponse<IEnumerable<MedicalMonitor>>(){ Data = cache};
            try
            {                
                var response = await masterRepository.GetMedicalMonitors();
                cache = response;
                return new TaskGenericResponse<IEnumerable<MedicalMonitor>>() { Data = response };
            }
            catch (Exception e)
            {
                ApiException errorException = e as ApiException;
                if (errorException != null)
                    return new TaskGenericResponse<IEnumerable<MedicalMonitor>>() { ErrorCode = errorException.Code, Message = errorException.Error };
                else
                    return new TaskGenericResponse<IEnumerable<MedicalMonitor>>() { ErrorCode = 600, Message = e.Message };
            }
        }
    }
}
