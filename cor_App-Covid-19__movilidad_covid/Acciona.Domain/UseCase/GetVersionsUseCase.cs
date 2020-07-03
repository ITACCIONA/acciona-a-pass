using Acciona.Domain.Model;
using Acciona.Domain.Model.Base;
using Acciona.Domain.Model.Employee;
using Acciona.Domain.Model.Versions;
using Acciona.Domain.Repository;
using ServiceLocator;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Acciona.Domain.UseCase
{
    public class GetVersionsUseCase
    {
        private IVersionsRepository versionsRepository;
        
        public GetVersionsUseCase()
        {
            versionsRepository = Locator.Current.GetService<IVersionsRepository>();
        }

        public async Task<TaskGenericResponse<VersionsInfo>> Execute()
        {            
            try
            {                
                var response = await versionsRepository.GetVersions();
                return new TaskGenericResponse<VersionsInfo>() { Data = response };
            }
            catch (Exception e)
            {
                ApiException errorException = e as ApiException;
                if (errorException != null)
                    return new TaskGenericResponse<VersionsInfo>() { ErrorCode = errorException.Code, Message = errorException.Error };
                else
                    return new TaskGenericResponse<VersionsInfo>() { ErrorCode = 600, Message = e.Message };
            }
        }
    }
}
