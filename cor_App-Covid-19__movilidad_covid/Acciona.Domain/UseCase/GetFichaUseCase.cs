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
    public class GetFichaUseCase
    {
        private IEmployeeRepository employeeRepository;
        
        public GetFichaUseCase()
        {
            employeeRepository = Locator.Current.GetService<IEmployeeRepository>();
        }

        public async Task<TaskGenericResponse<Ficha>> Execute()
        {            
            try
            {                
                var response = await employeeRepository.GetFicha();               
                return new TaskGenericResponse<Ficha>() { Data = response };
            }
            catch (Exception e)
            {
                ApiException errorException = e as ApiException;
                if (errorException != null)
                    return new TaskGenericResponse<Ficha>() { ErrorCode = errorException.Code, Message = errorException.Error };
                else
                    return new TaskGenericResponse<Ficha>() { ErrorCode = 600, Message = e.Message };
            }
        }
    }
}
