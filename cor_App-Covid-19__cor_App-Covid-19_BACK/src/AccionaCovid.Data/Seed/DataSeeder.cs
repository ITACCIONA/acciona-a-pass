using AccionaCovid.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AccionaCovid.Data.Seed
{
    /// <summary>
    /// Clase que añade tablas maestras en la BBDD
    /// </summary>
    public class DataSeeder
    {
        /// <summary>
        /// Contexto a usar
        /// </summary>
        private readonly AccionaCovidContext context;

        /// <summary>
        /// Instancia del logger a usar
        /// </summary>
        private readonly ILogger<DataSeeder> logger;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        /// <param name="environment"></param>
        /// <param name="logger"></param>
        public DataSeeder(AccionaCovidContext context, ILogger<DataSeeder> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// Loads the data
        /// </summary>
        public void Seed(JObject parsed)
        {
            try
            {
                LoadDbSet(parsed, nameof(context.Role), context.Role);
                LoadDbSet(parsed, nameof(context.ColorEstado), context.ColorEstado);
                LoadDbSet(parsed, nameof(context.TipoEstado), context.TipoEstado);
                LoadDbSet(parsed, nameof(context.EstadoPasaporte), context.EstadoPasaporte);
                LoadDbSet(parsed, nameof(context.Division), context.Division);
                LoadDbSet(parsed, nameof(context.TipoParametroMedico), context.TipoParametroMedico);
                LoadDbSet(parsed, nameof(context.TipoSintomas), context.TipoSintomas);
                LoadDbSet(parsed, nameof(context.FactorRiesgo), context.FactorRiesgo);
                LoadDbSet(parsed, nameof(context.Pais), context.Pais);
                LoadDbSet(parsed, nameof(context.Tecnologia), context.Tecnologia);
                LoadDbSet(parsed, nameof(context.EstadoObra), context.EstadoObra);

                //AssignRoleToUser();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error seeding");
            }
        }

        /// <summary>
        /// Metodo que carga en BBDD los datos de una entidad
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <param name="name"></param>
        /// <param name="set"></param>
        private void LoadDbSet<T>(JObject json, string name, DbSet<T> set) where T : class
        {
            try
            {
                if (set.Any()) return;

                foreach (var item in json[name].ToObject<List<T>>())
                {
                    set.Add(item);
                    context.SaveChanges(false);
                }
                context.SaveChanges(false);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "LoadDbSet");
            }
        }
    }
}
