using Acciona.Domain.Model;
using Acciona.Domain.Model.Base;
using Acciona.Domain.Model.Employee;
using Acciona.Domain.Repository;
using ServiceLocator;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Acciona.Domain.UseCase
{
    public class GetAlertsUseCase
    {
        private IEmployeeRepository employeeRepository;

        public GetAlertsUseCase()
        {
            employeeRepository = Locator.Current.GetService<IEmployeeRepository>();
        }

        public async Task<TaskGenericResponse<IEnumerable<Alert>>> Execute()
        {
            try
            {                
                var response = await employeeRepository.GetAlerts();
                
                return new TaskGenericResponse<IEnumerable<Alert>>() { Data = response };
            }
            catch (Exception e)
            {
                ApiException errorException = e as ApiException;
                if (errorException != null)
                    return new TaskGenericResponse<IEnumerable<Alert>>() { ErrorCode = errorException.Code, Message = errorException.Error };
                else
                    return new TaskGenericResponse<IEnumerable<Alert>>() { ErrorCode = 600, Message = e.Message };
            }
        }
    }
}
