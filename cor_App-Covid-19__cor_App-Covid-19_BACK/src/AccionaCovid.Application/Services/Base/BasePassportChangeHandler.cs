using AccionaCovid.Application.Core;
using AccionaCovid.Domain.Core;
using AccionaCovid.Domain.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccionaCovid.Application.Services
{
    /// <summary>
    /// Clase base para los handlers que implican cambios de estado en el pasaporte calculados a partir de la matriz
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    public abstract class BasePassportChangeHandler<TRequest, TResponse> : BaseCommandHandler<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        /// <summary>
        /// Repositorio de empleado
        /// </summary>
        private readonly IRepository<Empleado> repositoryEmpleado;

        /// <summary>
        /// Repositorio de ficha médica
        /// </summary>
        private readonly IRepository<FichaMedica> repositoryFichaMedica;

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="repositoryEmpleado">Repositorio de empelado</param>
        /// <param name="repositoryFichaMedica">Repositorio de ficha médica</param>
        public BasePassportChangeHandler(IRepository<Empleado> repositoryEmpleado, IRepository<FichaMedica> repositoryFichaMedica)
        {
            this.repositoryEmpleado = repositoryEmpleado;
            this.repositoryFichaMedica = repositoryFichaMedica;
        }

        /// <summary>
        /// Develve el último pasaporte (el pasaporte activo)
        /// </summary>
        /// <param name="idEmpleado">Identificador del empleado</param>
        /// <returns></returns>
        public async Task<Pasaporte> GetLastPassportAsync(int idEmpleado)
        {
            return await repositoryEmpleado.GetAll()
                     .Include(e => e.Pasaporte)
                         .ThenInclude(p => p.IdEstadoPasaporteNavigation)
                            .ThenInclude(ep => ep.IdColorEstadoNavigation)
                     .Where(e => e.Id == idEmpleado)
                     .SelectMany(e => e.Pasaporte)
                     .Where(p => !p.Deleted)
                     .FirstOrDefaultAsync(p => p.Activo == true).ConfigureAwait(false);
        }

        /// <summary>
        /// Devuelve la última valoración del parámetro FiebreAlata de la ficha médica
        /// </summary>
        /// <param name="idFichaMedica">Identificador de la ficha médica</param>
        /// <returns></returns>
        public async Task<ValoracionParametroMedico> GetLastFiebreAsync(int idFichaMedica)
        {
            return await repositoryFichaMedica.GetAll()
                    .Include(fm => fm.SeguimientoMedico)
                        .ThenInclude(sm => sm.ValoracionParametroMedico)
                    .Where(c => c.Id == idFichaMedica)
                    .SelectMany(fm => fm.SeguimientoMedico)
                    .Where(sm => !sm.Deleted)
                    .OrderByDescending(sm => sm.FechaSeguimiento)
                    .SelectMany(sm => sm.ValoracionParametroMedico)
                    .Where(vpm => !vpm.Deleted)
                    .Where(vpm => vpm.IdParametroMedicoNavigation.Nombre == ParametroMedico.ParameterTypes.TemperaturaAlta.ToString())
                    .FirstOrDefaultAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Devuelve los resultados PCR de la ficha médica
        /// </summary>
        /// <param name="idFichaMedica">Identificador de la ficha médica</param>
        /// <returns></returns>
        public async Task<List<ResultadoTestPcr>> GetAllResultadoPcrAsync(int idFichaMedica)
        {
            return await repositoryFichaMedica.GetAll()
                    .Include(fm => fm.ResultadoTestPcr)
                    .Where(c => c.Id == idFichaMedica)
                    .SelectMany(fm => fm.ResultadoTestPcr)
                    .Where(rtpcr => !rtpcr.Deleted)
                    .ToListAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Devuelve la última valoración de una analítica - IgG de una ficha médica
        /// </summary>
        /// <param name="idFichaMedica">Identificador de la ficha médica</param>
        /// <returns></returns>
        public async Task<ValoracionParametroMedico> GetLastAnalisticIgG(int idFichaMedica)
        {
            return await repositoryFichaMedica.GetAll()
                    .Include(fm => fm.SeguimientoMedico)
                        .ThenInclude(sm => sm.ValoracionParametroMedico)
                    .Where(c => c.Id == idFichaMedica)
                    .SelectMany(fm => fm.SeguimientoMedico)
                    .Where(sm => !sm.Deleted)
                    .OrderByDescending(sm => sm.FechaSeguimiento)
                    .SelectMany(sm => sm.ValoracionParametroMedico)
                    .Where(vpm => !vpm.Deleted)
                    .Where(vpm => vpm.IdParametroMedicoNavigation.Nombre == ParametroMedico.ParameterTypes.IgG.ToString())
                    .FirstOrDefaultAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Devuelve la última valoración de una analítica - IgG de una ficha médica
        /// </summary>
        /// <param name="idFichaMedica">Identificador de la ficha médica</param>
        /// <returns></returns>
        public async Task<ValoracionParametroMedico> GetLastAnalisticIgM(int idFichaMedica)
        {
            return await repositoryFichaMedica.GetAll()
                    .Include(fm => fm.SeguimientoMedico)
                        .ThenInclude(sm => sm.ValoracionParametroMedico)
                    .Where(c => c.Id == idFichaMedica)
                    .SelectMany(fm => fm.SeguimientoMedico)
                    .Where(sm => !sm.Deleted)
                    .OrderByDescending(sm => sm.FechaSeguimiento)
                    .SelectMany(sm => sm.ValoracionParametroMedico)
                    .Where(vpm => !vpm.Deleted)
                    .Where(vpm => vpm.IdParametroMedicoNavigation.Nombre == ParametroMedico.ParameterTypes.IgM.ToString())
                    .FirstOrDefaultAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Devuelve el último resultado de un test rápido de una ficha medica
        /// </summary>
        /// <param name="idFichaMedica">Identificador de la ficha médica</param>
        /// <returns></returns>
        public async Task<ResultadoTestMedico> GetLastTestRapidoAsync(int idFichaMedica)
        {
            return await repositoryFichaMedica.GetAll()
                    .Include(fm => fm.ResultadoTestMedico)
                    .Where(c => c.Id == idFichaMedica)
                    .SelectMany(fm => fm.ResultadoTestMedico)
                    .Where(rtm => !rtm.Deleted)
                    .OrderByDescending(x => x.FechaTest)
                    .FirstOrDefaultAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Devuelve los últimos resultados de la encuesta de síntomas de una ficha médica
        /// </summary>
        /// <param name="idFichaMedica">Identificador de la ficha médica</param>
        /// <returns></returns>
        public async Task<(ResultadoEncuestaSintomas Fiebre, ResultadoEncuestaSintomas Otros, ResultadoEncuestaSintomas Contacto)> GetLastResultadoEncuestaAsync(int idFichaMedica)
        {
            var ultimosResulList = await repositoryFichaMedica.GetAll()
                    .Include(fm => fm.ResultadoEncuestaSintomas)
                        .ThenInclude(rem => rem.IdTipoSintomaNavigation)
                    .Where(c => c.Id == idFichaMedica)
                    .SelectMany(fm => fm.ResultadoEncuestaSintomas)
                    .Where(res => !res.Deleted)
                    .ToListAsync().ConfigureAwait(false);
            //.GroupBy(res => res.LastActionDate)
            //.OrderByDescending(x => x.Key)
            //.FirstOrDefaultAsync().ConfigureAwait(false);

            var ultimosResul = ultimosResulList.GroupBy(res => res.GrupoRespuestas)
                    .Select(c => new { c.Key, value = c.ToList(), maxDate = c.Max(r => r.LastActionDate) })
                    .OrderByDescending(x => x.maxDate)
                    .FirstOrDefault();

            return GetLastResultadoEncuesta(ultimosResul?.value);
        }

        /// <summary>
        /// Devuelve los últimos resultados de la encuesta de síntomas de una ficha médica
        /// </summary>
        /// <param name="idFichaMedica">Identificador de la ficha médica</param>
        /// <returns></returns>
        public (ResultadoEncuestaSintomas Fiebre, ResultadoEncuestaSintomas Otros, ResultadoEncuestaSintomas Contacto) GetLastResultadoEncuesta(List<ResultadoEncuestaSintomas> ultimaEncuesta)
        {
            ResultadoEncuestaSintomas utiDeclFiebre = ultimaEncuesta?.SingleOrDefault(re => re.IdTipoSintomaNavigation.Nombre == TipoSintomas.ParameterTypes.Fiebre.ToString());
            ResultadoEncuestaSintomas utiDeclOtros = ultimaEncuesta?.SingleOrDefault(re => re.IdTipoSintomaNavigation.Nombre == TipoSintomas.ParameterTypes.OtrosSintomas.ToString());
            ResultadoEncuestaSintomas utiDeclContato = ultimaEncuesta?.SingleOrDefault(re => re.IdTipoSintomaNavigation.Nombre == TipoSintomas.ParameterTypes.Contacto.ToString());

            return (utiDeclFiebre, utiDeclOtros, utiDeclContato);
        }
    }
}
