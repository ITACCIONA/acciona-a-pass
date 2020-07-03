using Acciona.Domain.Model;
using Acciona.Domain.Model.Base;
using Acciona.Domain.Model.Employee;
using Acciona.Domain.Model.Versions;
using Acciona.Domain.Repository;
using ServiceLocator;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Acciona.Domain.UseCase
{
    public class GetResourceURLUseCase
    {
        private IResourcesRepository resourcesRepository;
        
        public GetResourceURLUseCase()
        {
            resourcesRepository = Locator.Current.GetService<IResourcesRepository>();
        }

        public string Execute(int state)
        {
            var appSession = Locator.Current.GetService<AppSession>();
            return resourcesRepository.GetURLHelpForState(state,appSession.Language);
        }
    }
}
