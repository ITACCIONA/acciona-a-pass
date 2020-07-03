 using Acciona.Domain.Services;
using Newtonsoft.Json;
using Acciona.Data.Repository;
using ServiceLocator;
using System;
using System.Collections.Generic;
using System.Text;
using Acciona.Data.Repository.SQLite;
using Acciona.Domain.Repository;

namespace Acciona.Data
{
    public class DataConfiguration
    {
        public static void Init()
        {
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                DateParseHandling = DateParseHandling.None
            };
            
            Locator.CurrentMutable.RegisterLazySingleton<ISQLite>(() => new SQLiteImpl());
            //Repositories            
            Locator.CurrentMutable.RegisterLazySingleton<ISQLiteRepository>(() => new SQLiteRepository());
            Locator.CurrentMutable.RegisterLazySingleton<IAdminRepository>(() => new AdminRepository());
            Locator.CurrentMutable.RegisterLazySingleton<IEmployeeRepository>(() => new EmployeeRepository());
            Locator.CurrentMutable.RegisterLazySingleton<IMasterRepository>(() => new MasterRepository());
            Locator.CurrentMutable.RegisterLazySingleton<ISecurityRepository>(() => new SecurityRepository());
            Locator.CurrentMutable.RegisterLazySingleton<IVersionsRepository>(() => new VersionsRepository());
            Locator.CurrentMutable.RegisterLazySingleton<IResourcesRepository>(() => new ResourcesRepository());
        }
    }
}
