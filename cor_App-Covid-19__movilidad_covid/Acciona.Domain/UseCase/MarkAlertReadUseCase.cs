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
    public class MarkAlertReadUseCase
    {
        private IEmployeeRepository employeeRepository;

        public MarkAlertReadUseCase()
        {
            employeeRepository = Locator.Current.GetService<IEmployeeRepository>();
        }

        public async Task<TaskGenericResponse> Execute(int idAlert)
        {
            try
            {                
                await employeeRepository.MarkAlertRead(idAlert);

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
