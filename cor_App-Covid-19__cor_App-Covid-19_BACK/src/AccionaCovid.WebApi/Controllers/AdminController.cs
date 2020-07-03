using AccionaCovid.Application.Services.Admin;
using AccionaCovid.WebApi.Core;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace AccionaCovid.WebApi.Controllers
{
    /// <summary>
    /// Servicios de administracion
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : MediatorBaseController
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mediator"></param>
        public AdminController(IMediator mediator) : base(mediator)
        {
        }

        #endregion

        /// <summary>
        /// Metodo que crea un bloque
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("login/medicalServices")]
        [Authorize(AuthenticationSchemes = "IdentityServer,AzureAD", Roles = "ServicioMedico,PRL,Comunicacion,RRHHDesc,RRHHCent,GestorContratas")]
        [ProducesResponseType(typeof(GenericResponse<GetUser.GetUserResponse>), 200)]
        public async Task<IActionResult> GetLoginMedicalServices()
        {
            bool ss = User.IsInRole("ServicioMedico");

            var response = await Mediator.Send(new GetUser.GetUserRequest());
            return ResponseHelper.CreateResponse(response);
        }

        /// <summary>
        /// Metodo que crea un bloque
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("login/employee")]
        [Authorize(AuthenticationSchemes = "IdentityServer,AzureAD")]
        [ProducesResponseType(typeof(GenericResponse<GetUser.GetUserResponse>), 200)]
        public async Task<IActionResult> GetLoginEmployee()
        {
            var response = await Mediator.Send(new GetUser.GetUserRequest());
            return ResponseHelper.CreateResponse(response);
        }

        /// <summary>
        /// Metodo que crea un empleado
        /// </summary>
        /// <param name="idEmpleado">Identificador del empleado</param>
        /// <returns></returns>
        [Obsolete("Usar [POST] api/medicarServices/externalEmployees")]
        [HttpPost]
        [Route("employee")]
        [Authorize(AuthenticationSchemes = "AzureAD", Roles = "GestorContratas")]
        [ProducesResponseType(typeof(bool), 200)]
        public async Task<IActionResult> CreateEmployee([FromBody]Application.Services.MedicalServices.CreateExternalEmployee.CreateExternalEmployeeRequest request)
        {
            return ResponseHelper.CreateResponse(await Mediator.Send(request).ConfigureAwait(false));
        }

    }
}