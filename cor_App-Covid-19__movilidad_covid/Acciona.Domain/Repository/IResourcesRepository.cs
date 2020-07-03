using Acciona.Domain.Model.Versions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Acciona.Domain.Repository
{
    public interface IResourcesRepository
    {
        string GetURLHelpForState(int state, string language);
    }
}
