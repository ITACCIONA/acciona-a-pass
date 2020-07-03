using Acciona.Data.Model;
using Acciona.Data.Model.Admin;
using Acciona.Data.Model.Mapper;
using Acciona.Data.Model.Versions;
using Acciona.Data.Repository.Base;
using Acciona.Domain.Model.Admin;
using Acciona.Domain.Model.Base;
using Acciona.Domain.Model.Versions;
using Acciona.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acciona.Data.Repository
{
    public class VersionsRepository : ApiBaseService, IVersionsRepository
    {
        public async Task<VersionsInfo> GetVersions()
        {
            string url = String.Concat(DataConstants.EndpointVersions,DataConstants.VersionsURL);

            var response = await MakeHttpCall<BaseResponse<VersionsInfoData>, string>(url, HttpVerbMethod.Get, null,noCache:true)
                .ConfigureAwait(false);            

            return VersionsMapper.MapVersionsInfo(response.data.ElementAt(0));
        }
    }
}
