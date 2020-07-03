using Acciona.Domain.Model.Versions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Acciona.Domain.Repository
{
    public interface IVersionsRepository
    {
        Task<VersionsInfo> GetVersions();
    }
}
