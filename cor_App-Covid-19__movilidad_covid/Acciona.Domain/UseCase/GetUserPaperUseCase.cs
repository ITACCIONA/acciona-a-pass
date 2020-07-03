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
    public class GetUserPaperUseCase
    {
        private ISecurityRepository securityRepository;

        public GetUserPaperUseCase()
        {
            securityRepository = Locator.Current.GetService<ISecurityRepository>();
        }

        public async Task<TaskGenericResponse<UserPaper>> Execute(string document,string phone)
        {
            try
            {
                var response = await securityRepository.GetUserPaper(document,phone);
                return new TaskGenericResponse<UserPaper>() { Data = response };
            }
            catch (Exception e)
            {
                ApiException errorException = e as ApiException;
                if (errorException != null)
                    return new TaskGenericResponse<UserPaper>() { ErrorCode = errorException.Code, Message = errorException.Error };
                else
                    return new TaskGenericResponse<UserPaper>() { ErrorCode = 600, Message = e.Message };
            }
        }
    }
}
