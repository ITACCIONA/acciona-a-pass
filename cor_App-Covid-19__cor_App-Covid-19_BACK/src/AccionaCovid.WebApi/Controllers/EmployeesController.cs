using AccionaCovid.Application.Services.Employees;
using AccionaCovid.Crosscutting;
using AccionaCovid.WebApi.Core;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace AccionaCovid.WebApi.Controllers
{
    /// <summary>
    /// Servicios de empleados
    /// </summary>
    [ApiController, Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = "IdentityServer", Roles = "Empleado")]
    public class EmployeesController : MediatorBaseController
    {
        private readonly IUserInfoAccesor userInfoAccesor;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mediator"></param>
        public EmployeesController(IMediator mediator, IUserInfoAccesor userInfoAccesor) : base(mediator)
        {
            this.userInfoAccesor = userInfoAccesor;
        }

        #region Con autenticacion

        #region POST

        /// <summary>
        /// Método que registra un sintomca para un empleado
        /// </summary>
        /// <returns></returns>
        [HttpPost("Self/Symptoms"), ProducesResponseType(typeof(bool), 200)]
        public async Task<IActionResult> RegisterSymptomInquiry(RegisterSymptomInquiryResult.RegisterSymptomInquiryResultRequest request)
        {
            request.IdEmployee = userInfoAccesor.IdUser;
            return ResponseHelper.CreateResponse(await Mediator.Send(request).ConfigureAwait(false));
        }

        /// <summary>
        /// Metodo que crea un listado de factor de riesgo para un empleado
        /// </summary>
        /// <param name="idEmpleado">Identificador del empleado</param>
        /// <returns></returns>
        [HttpPost]
        [Route("Self/riskFactor")]
        [ProducesResponseType(typeof(bool), 200)]
        public async Task<IActionResult> CreateRiskFactors([FromBody]CreateRiskFactors.CreateRiskFactorsRequest request)
        {
            request.IdEmployee = userInfoAccesor.IdUser;
            return ResponseHelper.CreateResponse(await Mediator.Send(request).ConfigureAwait(false));
        }

        /// <summary>
        /// Metodo que crea un listado de factor de riesgo para un empleado
        /// </summary>
        /// <param name="idEmpleado">Identificador del empleado</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("Self/localizations"), ProducesResponseType(typeof(bool), 200)]
        public async Task<IActionResult> AddLocationToEmployee([FromBody]AddLocationToEmployee.AddLocationToEmployeeRequest request)
        {
            request.IdEmployee = userInfoAccesor.IdUser;
            return ResponseHelper.CreateResponse(await Mediator.Send(request).ConfigureAwait(false));
        }

        #endregion

        #region PUT

        /// <summary>
        /// Metodo que actualiza una alerta a leido
        /// </summary>
        /// <param name="idEmpleado">Identificador del empleado</param>
        /// <returns></returns>
        [HttpPut]
        [Route("Self/alert/{idAlert}")]
        [ProducesResponseType(typeof(bool), 200)]
        public async Task<IActionResult> AlertRead(int idAlert)
        {
            AlertRead.AlertReadRequest request = new AlertRead.AlertReadRequest()
            {
                IdAlert = idAlert,
                IdEmployee = userInfoAccesor.IdUser
            };

            return ResponseHelper.CreateResponse(await Mediator.Send(request).ConfigureAwait(false));
        }

        /// <summary>
        /// Metodo que actualiza una alerta a leido
        /// </summary>
        /// <param name="idEmpleado">Identificador del empleado</param>
        /// <returns></returns>
        [HttpPut("Self/ficha"), ProducesResponseType(typeof(bool), 200)]
        public async Task<IActionResult> UpdateEmployeeProfile([FromBody] UpdateEmployeeProfile.UpdateEmployeeProfileRequest request)
        {
            request.IdEmployee = userInfoAccesor.IdUser;
            return ResponseHelper.CreateResponse(await Mediator.Send(request).ConfigureAwait(false));
        }

        #endregion

        #region GET

        /// <summary>
        /// Método que obtiene el pasaporte de un empleado
        /// </summary>
        /// <returns></returns>
        [HttpGet("Self/Passport"), ProducesResponseType(typeof(GenericResponse<GetPassportByIdEmployee.GetPassportByIdEmployeeResponse>), 200)]
        public async Task<IActionResult> GetPasaporte() => ResponseHelper.CreateResponse(await Mediator.Send(new GetPassportByIdEmployee.GetPassportByIdEmployeeRequest() { IdEmployee = userInfoAccesor.IdUser }).ConfigureAwait(false));

        /// <summary>
        /// Método que obtiene el pasaporte de un empleado
        /// </summary>
        /// <returns></returns>
        [HttpGet("Self/alerts"), ProducesResponseType(typeof(GenericResponse<GetAlertsByEmployee.GetAlertsByEmployeeResponse>), 200)]
        public async Task<IActionResult> GetAlertsByEmployee() => ResponseHelper.CreateResponse(await Mediator.Send(new GetAlertsByEmployee.GetAlertsByEmployeeRequest() { IdEmployee = userInfoAccesor.IdUser }).ConfigureAwait(false));

        /// <summary>
        /// Método que obtiene el pasaporte de un empleado
        /// </summary>
        /// <returns></returns>
        [HttpGet("Self/ficha"), ProducesResponseType(typeof(GenericResponse<GetEmployeeProfile.GetEmployeeProfileResponse>), 200)]
        public async Task<IActionResult> GetEmployeeProfile() => ResponseHelper.CreateResponse(await Mediator.Send(new GetEmployeeProfile.GetEmployeeProfileRequest() { IdEmployee = userInfoAccesor.IdUser }).ConfigureAwait(false));

        /// <summary>
        /// Metodo que actualiza una alerta a leido
        /// </summary>
        /// <param name="idEmpleado">Identificador del empleado</param>
        /// <param name="idAlert">Identificador de la alerta</param>
        /// <returns></returns>
        [HttpPut]
        [Route("Self/panic")]
        [ProducesResponseType(typeof(bool), 200)]
        public async Task<IActionResult> PanicRegister(DateTimeOffset currentDeviceDateTime) => ResponseHelper.CreateResponse(await Mediator.Send(new PanicRegister.PanicRegisterRequest() { IdEmployee = userInfoAccesor.IdUser, CurrentDeviceDateTime = currentDeviceDateTime }).ConfigureAwait(false));

        /// <summary>
        /// Método que obtiene el pasaporte de un empleado
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("Self/localizations"), ProducesResponseType(typeof(GenericResponse<GetLocationsByEmployee.GetLocationsByEmployeeResponse>), 200)]
        public async Task<IActionResult> GetLocationsByEmployee() => ResponseHelper.CreateResponse(await Mediator.Send(new GetLocationsByEmployee.GetLocationsByEmployeeRequest() { IdEmployee = userInfoAccesor.IdUser }).ConfigureAwait(false));

        #endregion

        #endregion
    }
}