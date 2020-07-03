using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Acciona.Domain.Model;
using Acciona.Domain.Repository;
using Acciona.Domain.Model.Base;
using ServiceLocator;
using Newtonsoft.Json;
using System.Linq;

namespace Acciona.Domain.UseCase
{
    public class SincroPendingUseCase
    {
        private ISQLiteRepository sqliteRepository;
        private ISecurityRepository securityRepository;
        private bool working = false;

        public SincroPendingUseCase()
        {
            sqliteRepository = Locator.Current.GetService<ISQLiteRepository>();
            securityRepository = Locator.Current.GetService<ISecurityRepository>();            
        }

        public async Task<TaskGenericResponse> Execute()
        {
            while (working)
                await Task.Delay(200);
            working = true;
            var response = new TaskGenericResponse();
            var pending=sqliteRepository.GetPendingSincro().ToList();
            var appSession = Locator.Current.GetService<AppSession>();
            /*if (appSession.AccesToken == null) //avoid Connection chnage service sincro without token
            {
                working = false;
                return response;
            }*/
            foreach (var p in pending)
            {                
               if (p.Type == Sincro.TYPE_TEMPERATURE)
                {
                    try
                    {
                        var temperatureData = JsonConvert.DeserializeObject<TemperatureSincro>(p.Serialized);
                        await securityRepository.SendTemperature(temperatureData.IdEmpleado, temperatureData.IdDevice, temperatureData.IsTemperatureOver, new DateTime(temperatureData.Date));
                          
                    }
                    catch (Exception e)
                    {
                        /*ApiException errorException = e as ApiException;

                        if (errorException != null)
                        {
                            response.ErrorCode = errorException.Code;
                            response.Message = errorException.Error;
                        }
                        else
                        {
                            response.ErrorCode = 600;
                            if (e.Message.StartsWith("Unable to resolve host"))
                                response.Message = "Por favor verifique su conexión";
                            else
                                response.Message = e.Message;
                        }
                        break;                        */
                    }
                }                
                sqliteRepository.DeleteItem<Sincro>(p);
            }
            working = false;
            return response;
        }
    }
}
