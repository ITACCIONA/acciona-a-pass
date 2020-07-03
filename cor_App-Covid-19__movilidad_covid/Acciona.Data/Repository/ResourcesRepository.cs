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
    public class ResourcesRepository : IResourcesRepository
    {
        public string GetURLHelpForState(int state, string language)
        {            
            string url = String.Concat(DataConstants.EndpointVersions,String.Format(DataConstants.ResourcesURL, state,language.ToUpper()));            
            return url;
        }
    }
}
