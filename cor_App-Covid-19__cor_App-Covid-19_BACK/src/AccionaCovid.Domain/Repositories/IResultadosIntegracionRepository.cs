using AccionaCovid.Domain.Core;
using AccionaCovid.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace AccionaCovid.Domain.Repositories
{
    public interface IResultadosIntegracionRepository: IRepository<ResultadosIntegracion>
    {
        /// <summary>
        /// Nombre del archivo de origen que compartirá la instacia de repositorio
        /// </summary>
        string ProcessedFileName { get; set; }

        /// <summary>
        /// Fecha de registro de las trazas compartidas por la instancia del repositorio
        /// </summary>
        DateTime IntegrationProcessDateTime { get; set; }

        /// <summary>
        /// Agrega un mensaje que no es error a la traza de integración
        /// </summary>
        /// <param name="bloque">Bloque de procesamiento de la integración</param>
        /// <param name="message">Mensaje a registrar</param>
        void AddMessage(string bloque, string message);

        /// <summary>
        /// Agrega un error a la traza de la integración
        /// </summary>
        /// <param name="bloque">Bloque de procesamiento de la integración</param>
        /// <param name="message">Mensaje a registrar</param>
        void AddError(string bloque, string message);

        /// <summary>
        /// Agrega un mensaje a la traza de la integración
        /// </summary>
        /// <typeparam name="T">Entidad del modelo</typeparam>
        /// <param name="bloque">Bloque de procesamientode la integración</param>
        /// <param name="entityList">Lista de registros insertados o actualizados</param>
        void AddCountInsert<T>(string bloque, List<T> entityList) where T: Entity<T>;


        /// <summary>
        /// Agrega un mensaje a la traza de la integración
        /// </summary>
        /// <typeparam name="T">Entidad del modelo</typeparam>
        /// <param name="bloque">Bloque de procesamientode la integración</param>
        /// <param name="entityList">Lista de registros insertados o actualizados</param>
        void AddCountDelete<T>(string bloque, List<T> entityList) where T : Entity<T>;


        /// <summary>
        /// Agrega un mensaje a la traza de la integración
        /// </summary>
        /// <typeparam name="T">Entidad del modelo</typeparam>
        /// <param name="bloque">Bloque de procesamientode la integración</param>
        /// <param name="entityList">Lista de registros insertados o actualizados</param>
        void AddCountUpdate<T>(string bloque, List<T> entityList) where T : Entity<T>;
    }
}
