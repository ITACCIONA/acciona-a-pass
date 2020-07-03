using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Acciona.Domain.Model;
using Acciona.Domain.Repository;
using ServiceLocator;
using Newtonsoft.Json;

namespace Acciona.Domain.UseCase
{
    public class SaveTemperatureUseCase
    {
        private ISQLiteRepository sqliteRepository;
        private SincroPendingUseCase sincroPendingUseCase;

        public SaveTemperatureUseCase()
        {
            sqliteRepository = Locator.Current.GetService<ISQLiteRepository>();
            sincroPendingUseCase = Locator.Current.GetService<SincroPendingUseCase>();
        }

        public void Execute(TemperatureSincro info)
        {            
            Sincro s = new Sincro();
            s.User = "Security";
            s.Time = DateTime.Now.Ticks;
            s.Type = Sincro.TYPE_TEMPERATURE;
            s.Serialized = JsonConvert.SerializeObject(info);
            sqliteRepository.AddItem<Sincro>(s);
            Task.Run(() => sincroPendingUseCase.Execute());
        }
    }
}
