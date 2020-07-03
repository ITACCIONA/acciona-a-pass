using AccionaCovid.Crosscutting;
using AccionaCovid.Domain.Core;
using AccionaCovid.Domain.Model;
using AccionaCovid.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace AccionaCovid.Data.Repositories
{
    public class ResultadosIntegracionRepository : GenericRepository<ResultadosIntegracion>, IResultadosIntegracionRepository
    {
        public string ProcessedFileName { get; set; }
        public DateTime IntegrationProcessDateTime { get; set; }

        public ResultadosIntegracionRepository(AccionaCovidContext context, IUserInfoAccesor userInfoAccesor, bool logicalRemove) : 
            base(context, userInfoAccesor, logicalRemove) { }

        /// <summary>
        /// Agrega un mensaje que no es error a la traza de integración
        /// </summary>
        /// <param name="bloque">Bloque de procesamiento de la integración</param>
        /// <param name="message">Mensaje a registrar</param>
        public void AddMessage(string bloque, string message)
        {
            Context.Add(new ResultadosIntegracion()
            {
                Bloque = bloque,
                EsError = false,
                Fecha = IntegrationProcessDateTime,
                OriginFileName = ProcessedFileName,
                Mensaje = message
            });
        }

        public void AddCountDelete<T>(string bloque, List<T> entityList) where T : Entity<T>
        {
            Context.Add(new ResultadosIntegracion()
            {
                Bloque = bloque,
                EsError = false,
                Fecha = IntegrationProcessDateTime,
                OriginFileName = ProcessedFileName,
                Mensaje = $"Se han marcado para eliminar {entityList.Count} registros de {typeof(T).Name}."
            });
        }

        public void AddCountInsert<T>(string bloque, List<T> entityList) where T : Entity<T>
        {
            Context.Add(new ResultadosIntegracion()
            {
                Bloque = bloque,
                EsError = false,
                Fecha = IntegrationProcessDateTime,
                OriginFileName = ProcessedFileName,
                Mensaje = $"Se han insertado {entityList.Count} registros de {typeof(T).Name} nuevos."
            });
        }

        public void AddCountUpdate<T>(string bloque, List<T> entityList) where T : Entity<T>
        {
            Context.Add(new ResultadosIntegracion()
            {
                Bloque = bloque,
                EsError = false,
                Fecha = IntegrationProcessDateTime,
                OriginFileName = ProcessedFileName,
                Mensaje = $"Se han actualizado {entityList.Count} registros de {typeof(T).Name}."
            });
        }

        public void AddError(string bloque, string message)
        {
            Context.Add(new ResultadosIntegracion()
            {
                Bloque = bloque,
                EsError = true,
                Fecha = IntegrationProcessDateTime,
                OriginFileName = ProcessedFileName,
                Mensaje = message
            });
        }
    }
}
