using AccionaCovid.Application.Services.SecurityScan;
using AccionaCovid.WebApi.Core;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace AccionaCovid.WebApi.Controllers
{
    /// <summary>
    /// Servicios de escaneos de seguridad
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "ApiKey")]
    public class SecurityScanController : MediatorBaseController
    {

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mediator"></param>
        public SecurityScanController(IMediator mediator) : base(mediator)
        {
        }

        #endregion

        #region POST

        /// <summary>
        /// Metodo que obtiene los test medicos de un empleado
        /// </summary>
        /// <param name="idEmpleado">Identificador del empleado</param>
        /// <returns></returns>
        [HttpPost("Employees/{idEmpleado}/TemperatureMedition")]
        [ProducesResponseType(typeof(bool), 200)]
        public async Task<IActionResult> RegisterTemperatureMedition(int idEmpleado, [FromBody]RegisterTemperatureMeditionSecurity.RegisterTemperatureMeditionSecurityRequest request)
        {
            request.IdEmployee = idEmpleado;
            return ResponseHelper.CreateResponse(await Mediator.Send(request).ConfigureAwait(false));
        }


        /// <summary>
        /// Metodo que genera pasaporte (Rojo verde o verde papel)
        /// </summary>
        /// <param name="idEmpleado">Identificador del empleado</param>
        /// <returns></returns>
        [HttpPost("Employees/{idEmpleado}/GenerationManual")]
        [ProducesResponseType(typeof(bool), 200)]
        public async Task<IActionResult> RegisterGenerationManual(int idEmpleado, [FromBody]RegisterGenerationManual.RegisterGenerationManualRequest request)
        {
            request.IdEmployee = idEmpleado;
            return ResponseHelper.CreateResponse(await Mediator.Send(request).ConfigureAwait(false));
        }

        #endregion

        #region GET

        /// <summary>
        /// Método que obtiene el pasaporte activo de un empleado
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("employee/{idEmpleado}/Passports/Active"), ProducesResponseType(typeof(GenericResponse<GetActivePassport.GetActivePassportResponse>), 200)]
        public async Task<IActionResult> GetActivePasaporte(int idEmpleado) => ResponseHelper.CreateResponse(await Mediator.Send(new GetActivePassport.GetActivePassportRequest { IdEmployee = idEmpleado }).ConfigureAwait(false));


        /// <summary>
        /// Método que obtiene el empleado de forma manual
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("employee"), ProducesResponseType(typeof(GenericResponse<GetEmployeeManualProfile.GetEmployeeManualProfileResponse>), 200)]
        public async Task<IActionResult> GetEmpleadoManual(string dniEmpleado, string telefonoEmpleado)
        {
          return ResponseHelper.CreateResponse(await Mediator.Send(new GetEmployeeManualProfile.GetEmployeeManualProfileRequest { DniEmpleado = dniEmpleado, TelefonoEmpleado = telefonoEmpleado }).ConfigureAwait(false));
        }

        /// <summary>
        /// Metodo que obtiene las localizaciones disponibles
        /// </summary>
        /// <returns></returns>
        [HttpGet("locations"), ProducesResponseType(typeof(GenericResponse<GetLocations.GetLocationsSecurityResponse>), 200)]
         public async Task<IActionResult> GetLocations()
        {
               return ResponseHelper.CreateResponse(await Mediator.Send(new GetLocations.GetLocationsRequest()).ConfigureAwait(false));
        }
           



        #endregion
    }
}