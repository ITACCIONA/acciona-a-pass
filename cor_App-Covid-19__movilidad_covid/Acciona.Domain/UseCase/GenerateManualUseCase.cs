using Acciona.Domain.Model;
using Acciona.Domain.Model.Security;
using Acciona.Domain.Model.Base;
using Acciona.Domain.Repository;
using ServiceLocator;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Acciona.Domain.UseCase
{
    public class GenerateManualUseCase
    {
        private ISecurityRepository securityRepository;

        public GenerateManualUseCase()
        {
            securityRepository = Locator.Current.GetService<ISecurityRepository>();
        }

        public async Task<TaskGenericResponse> Execute(int idEmpleado, bool isGreenPaper)
        {
            try
            {
                await securityRepository.GenerateManual(idEmpleado,isGreenPaper,DateTime.Now);
                return new TaskGenericResponse();
            }
            catch (Exception e)
            {
                ApiException errorException = e as ApiException;
                if (errorException != null)
                    return new TaskGenericResponse() { ErrorCode = errorException.Code, Message = errorException.Error };
                else
                    return new TaskGenericResponse() { ErrorCode = 600, Message = e.Message };
            }
        }
    }
}
