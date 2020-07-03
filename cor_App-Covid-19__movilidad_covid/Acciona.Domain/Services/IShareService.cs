using System;
namespace Acciona.Domain.Services
{
        public enum TargetApp
        {
            SkypeEnterprise,
            Email
        }

        public interface IShareService
        {
            bool ShareApp(TargetApp targetApp);
        }

    
}
