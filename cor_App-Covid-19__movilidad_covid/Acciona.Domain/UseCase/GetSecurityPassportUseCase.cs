using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Acciona.Domain.Model;
using Acciona.Domain.Repository;
using ServiceLocator;
using Newtonsoft.Json;
using Acciona.Domain.Model.Base;
using Acciona.Domain.Model.Employee;

namespace Acciona.Domain.UseCase
{
    public class GetSecurityPassportUseCase
    {
        private ISQLiteRepository sqliteRepository;
        private ISecurityRepository securityRepository;

        public GetSecurityPassportUseCase()
        {
            sqliteRepository = Locator.Current.GetService<ISQLiteRepository>();
            securityRepository = Locator.Current.GetService<ISecurityRepository>();
        }

        public async Task<TaskGenericResponse<Passport>> Execute(int idEmpleado)
        {
            try
            {
                var response = await securityRepository.GetSecurityPassport(idEmpleado);
                return new TaskGenericResponse<Passport>() { Data = response };
            }
            catch (Exception e)
            {
                ApiException errorException = e as ApiException;
                if (errorException != null)
                {
                    return new TaskGenericResponse<Passport>() { ErrorCode = errorException.Code, Message = errorException.Error };
                }
                else
                    return new TaskGenericResponse<Passport>() { ErrorCode = 600, Message = e.Message };
            }
        }
    }
}
