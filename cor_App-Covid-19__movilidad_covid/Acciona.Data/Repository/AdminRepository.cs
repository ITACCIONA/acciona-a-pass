using Acciona.Data.Model;
using Acciona.Data.Model.Admin;
using Acciona.Data.Model.Mapper;
using Acciona.Data.Repository.Base;
using Acciona.Domain.Model.Admin;
using Acciona.Domain.Model.Base;
using Acciona.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acciona.Data.Repository
{
    public class AdminRepository : ApiBaseService, IAdminRepository
    {
        public async Task<User> GetUser()
        {
            string url = String.Concat(DataConstants.Endpoint,DataConstants.UserURL);

            var response = await MakeSessionHttpCall<BaseResponse<UserData>, string>(url, HttpVerbMethod.Get, null)
                .ConfigureAwait(false);
            if (response.HasError)
            {
                if (response.info != null && response.info.Count() > 0)
                    response.Message = String.Join("\n", response.info.Select(x=>x.message));
                throw new ApiException()
                {
                    Code = response.status,
                    Error = response.Message
                };
            }

            return AdminMapper.MapUser(response.data.ElementAt(0));
        }
    }
}
