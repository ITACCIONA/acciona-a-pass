using Acciona.Domain.Model;
using Acciona.Domain.Model.Admin;
using Acciona.Domain.Model.Base;
using Acciona.Domain.Repository;
using ServiceLocator;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Acciona.Domain.UseCase
{
    public class GetUserUseCase
    {
        private IAdminRepository adminRepository;

        public GetUserUseCase()
        {
            adminRepository = Locator.Current.GetService<IAdminRepository>();
        }

        public async Task<TaskGenericResponse<User>> Execute()
        {
            try
            {
                var response = await adminRepository.GetUser();
                var appSession = Locator.Current.GetService<AppSession>();
                appSession.UserInfo = response;
                return new TaskGenericResponse<User>() { Data = response };
            }
            catch (Exception e)
            {
                ApiException errorException = e as ApiException;
                if (errorException != null)
                    return new TaskGenericResponse<User>() { ErrorCode = errorException.Code, Message = errorException.Error };
                else
                    return new TaskGenericResponse<User>() { ErrorCode = 600, Message = e.Message };
            }
        }
    }
}
