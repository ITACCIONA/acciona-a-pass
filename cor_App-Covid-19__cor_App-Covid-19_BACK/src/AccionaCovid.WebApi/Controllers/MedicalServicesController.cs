using AccionaCovid.Application.Services.MedicalServices;
using AccionaCovid.Crosscutting;
using AccionaCovid.WebApi.Core;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccionaCovid.WebApi.Controllers
{
    /// <summary>
    /// Servicios de servicios medicos
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class MedicalServicesController : MediatorBaseController
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mediator"></param>
        public MedicalServicesController(IMediator mediator) : base(mediator)
        {
        }

        #endregion

        #region POST

        /// <summary>
        /// Metodo que obtiene los test medicos de un empleado
        /// </summary>
        /// <param name="idEmpleado">Identificador del empleado</param>
        /// <returns></returns>
        [HttpPost("employee/{idEmpleado}/TemperatureMedition")]
        [ProducesResponseType(typeof(bool), 200)]
        [Authorize(AuthenticationSchemes = "AzureAD", Roles = "ServicioMedico,PRL,RRHHDesc,RRHHCent")]
        public async Task<IActionResult> RegisterTemperatureMedition(int idEmpleado, [FromBody]RegisterTemperatureMedition.RegisterTemperatureMeditionRequest request)
        {
            request.IdEmployee = idEmpleado;
            return ResponseHelper.CreateResponse(await Mediator.Send(request).ConfigureAwait(false));
        }

        /// <summary>
        /// Metodo que registra test rapidos
        /// </summary>
        /// <param name="idEmpleado">Identificador del empleado</param>
        /// <returns></returns>
        [HttpPost]
        [Route("employee/{idEmpleado}/QuickTest")]
        [ProducesResponseType(typeof(bool), 200)]
        [Authorize(AuthenticationSchemes = "AzureAD", Roles = "ServicioMedico,PRL,RRHHDesc,RRHHCent")]
        public async Task<IActionResult> RegisterQuickTest(int idEmpleado, [FromBody]RegisterQuickTest.RegisterQuickTestRequest request)
        {
            request.IdEmployee = idEmpleado;
            return ResponseHelper.CreateResponse(await Mediator.Send(request).ConfigureAwait(false));
        }

        /// <summary>
        /// Metodo que registra test PCR
        /// </summary>
        /// <param name="idEmpleado">Identificador del empleado</param>
        /// <returns></returns>
        [HttpPost]
        [Route("employee/{idEmpleado}/pcrTest")]
        [ProducesResponseType(typeof(bool), 200)]
        [Authorize(AuthenticationSchemes = "AzureAD", Roles = "ServicioMedico,PRL,RRHHDesc,RRHHCent")]
        public async Task<IActionResult> RegisterPCRTest(int idEmpleado, [FromBody]RegisterPCRTest.RegisterPCRTestRequest request)
        {
            request.IdEmployee = idEmpleado;
            return ResponseHelper.CreateResponse(await Mediator.Send(request).ConfigureAwait(false));
        }

        /// <summary>
        /// Metodo que registra seguimientos medicos de tipo imagen o analitica
        /// </summary>
        /// <param name="idEmpleado">Identificador del empleado</param>
        /// <returns></returns>
        [HttpPost]
        [Route("employee/{idEmpleado}/monitoring")]
        [ProducesResponseType(typeof(bool), 200)]
        [Authorize(AuthenticationSchemes = "AzureAD", Roles = "ServicioMedico,PRL,RRHHDesc,RRHHCent")]
        public async Task<IActionResult> RegisterMedicalMonitoring(int idEmpleado, [FromBody]RegisterMedicalMonitoring.RegisterMedicalMonitoringRequest request)
        {
            request.IdEmployee = idEmpleado;
            return ResponseHelper.CreateResponse(await Mediator.Send(request).ConfigureAwait(false));
        }

        /// <summary>
        /// Método que registra un nuevo sintoma a un empleado
        /// </summary>
        /// <returns></returns>
        [HttpPost("employee/{idEmployee}/Passport"), ProducesResponseType(typeof(bool), 200)]
        [Authorize(AuthenticationSchemes = "AzureAD", Roles = "ServicioMedico,PRL,RRHHDesc,RRHHCent")]
        public async Task<IActionResult> CreatePassport(int idEmployee, CreatePassport.CreatePassportRequest request)
        {
            request.IdEmployee = idEmployee;
            return ResponseHelper.CreateResponse(await Mediator.Send(request).ConfigureAwait(false));
        }

        /// <summary>
        /// Método que registra un nuevo sintoma a un empleado
        /// </summary>
        /// <returns></returns>
        [HttpPost("employee/{idEmployee}/Passport/prlred"), ProducesResponseType(typeof(bool), 200)]
        [Authorize(AuthenticationSchemes = "AzureAD", Roles = "PRL,GestorContratas")]
        public async Task<IActionResult> CreatePassportPrlRed(int idEmployee, CreatePassportByStateString.CreatePassportByStateStringRequest request)
        {
            request.IdEmployee = idEmployee;
            request.StateString = "Rojo-PRL";
            return ResponseHelper.CreateResponse(await Mediator.Send(request).ConfigureAwait(false));
        }

        /// <summary>
        /// Método que registra un nuevo sintoma a un empleado
        /// </summary>
        /// <returns></returns>
        [HttpPost("employee/{idEmployee}/Passport/leavered"), ProducesResponseType(typeof(bool), 200)]
        [Authorize(AuthenticationSchemes = "AzureAD", Roles = "RRHHDesc")]
        public async Task<IActionResult> CreatePassportLeaveRed(int idEmployee, CreatePassportByStateString.CreatePassportByStateStringRequest request)
        {
            request.IdEmployee = idEmployee;
            request.StateString = "Rojo-Baja";
            return ResponseHelper.CreateResponse(await Mediator.Send(request).ConfigureAwait(false));
        }

        /// <summary>
        /// Método que registra un nuevo sintoma a un empleado
        /// </summary>
        /// <returns></returns>
        [HttpPost("employee/{idEmployee}/Passport/expired"), ProducesResponseType(typeof(bool), 200)]
        [Authorize(AuthenticationSchemes = "AzureAD", Roles = "PRL,RRHHCent,GestorContratas")]
        public async Task<IActionResult> CreatePassportExpired(int idEmployee, CreatePassportByStateString.CreatePassportByStateStringRequest request)
        {
            request.IdEmployee = idEmployee;
            request.StateString = "DeclaracionCaducada1";
            return ResponseHelper.CreateResponse(await Mediator.Send(request).ConfigureAwait(false));
        }

        // <summary>
        /// Metodo que crea una alerta a todos ls empelados de la compañia
        /// </summary>
        /// <returns></returns>
        [HttpPost("alert/employees")]
        [ProducesResponseType(typeof(bool), 200)]
        [Authorize(AuthenticationSchemes = "AzureAD", Roles = "ServicioMedico,Comunicacion")]
        public async Task<IActionResult> CreateAlertAllEmployee([FromBody]CreateAlertAllEmployee.CreateAlertAllEmployeeRequest request) => ResponseHelper.CreateResponse(await Mediator.Send(request).ConfigureAwait(false));

        // <summary>
        /// Metodo que crea una alerta a todos ls empelados de un pais
        /// </summary>
        /// <returns></returns>
        [HttpPost("alert/country")]
        [ProducesResponseType(typeof(bool), 200)]
        [Authorize(AuthenticationSchemes = "AzureAD", Roles = "ServicioMedico,Comunicacion")]
        public async Task<IActionResult> CreateAlertByCountry([FromBody]CreateAlertByCountry.CreateAlertByCountryRequest request) => ResponseHelper.CreateResponse(await Mediator.Send(request).ConfigureAwait(false));

        // <summary>
        /// Metodo que crea una alerta a todos ls empelados de una division
        /// </summary>
        /// <returns></returns>
        [HttpPost("alert/division")]
        [ProducesResponseType(typeof(bool), 200)]
        [Authorize(AuthenticationSchemes = "AzureAD", Roles = "ServicioMedico,Comunicacion")]
        public async Task<IActionResult> CreateAlertByDivision([FromBody]CreateAlertByDivision.CreateAlertByDivisionRequest request) => ResponseHelper.CreateResponse(await Mediator.Send(request).ConfigureAwait(false));

        // <summary>
        /// Metodo que crea una alerta a un empleado
        /// </summary>
        /// <returns></returns>
        [HttpPost("alert/employee/{idEmployee}")]
        [ProducesResponseType(typeof(bool), 200)]
        [Authorize(AuthenticationSchemes = "AzureAD", Roles = "ServicioMedico")]
        public async Task<IActionResult> CreateAlertByEmployee(int idEmployee, [FromBody]CreateAlertByEmployee.CreateAlertByEmployeeRequest request)
        {
            request.IdEmployee = idEmployee;
            return ResponseHelper.CreateResponse(await Mediator.Send(request).ConfigureAwait(false));
        }

        // <summary>
        /// Metodo que crea una alerta a todos ls empelados de una division
        /// </summary>
        /// <returns></returns>
        [HttpPost("alert/location")]
        [ProducesResponseType(typeof(bool), 200)]
        [Authorize(AuthenticationSchemes = "AzureAD", Roles = "ServicioMedico,Comunicacion")]
        public async Task<IActionResult> CreateAlertByLocation([FromBody]CreateAlertByLocation.CreateAlertByLocationRequest request) => ResponseHelper.CreateResponse(await Mediator.Send(request).ConfigureAwait(false));

        /// <summary>
        /// Metodo que crea un empleado externo
        /// </summary>
        /// <param name="idEmpleado">Identificador del empleado</param>
        /// <returns></returns>
        [HttpPost]
        [Route("externalEmployees")]
        [ProducesResponseType(typeof(bool), 200)]
        [Authorize(AuthenticationSchemes = "AzureAD", Roles = "GestorContratas")]
        public async Task<IActionResult> CreateExternalEmployee([FromBody]CreateExternalEmployee.CreateExternalEmployeeRequest request) => ResponseHelper.CreateResponse(await Mediator.Send(request).ConfigureAwait(false));

        #endregion

        #region GET

        /// <summary>
        /// Metodo que crea un bloque
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("employees")]
        [ProducesResponseType(typeof(GenericResponse<GetEmployees.GetEmployeesResponse>), 200)]
        [Authorize(AuthenticationSchemes = "AzureAD", Roles = "ServicioMedico,PRL,RRHHDesc,RRHHCent")]
        public async Task<IActionResult> GetAllEmployees(string sortOrder, string nombre, bool? orderByDescending, int? page) => ResponseHelper.CreateResponse(await Mediator.Send(new GetEmployees.GetEmployeesRequest { SortOrder = sortOrder, Nombre = nombre, OrderByDescending = orderByDescending, Page = page }).ConfigureAwait(false));

        /// <summary>
        /// Metodo que crea un bloque
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("employees/search")]
        [ProducesResponseType(typeof(GenericResponse<GetEmployees.GetEmployeesResponse>), 200)]
        [Authorize(AuthenticationSchemes = "AzureAD", Roles = "ServicioMedico,PRL,RRHHDesc,RRHHCent")]
        public async Task<IActionResult> GetAllEmployees([FromBody]GetEmployees.GetEmployeesRequest request) => ResponseHelper.CreateResponse(await Mediator.Send(request).ConfigureAwait(false));

        /// <summary>
        /// Metodo que crea un bloque
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [FormatFilter]
        [Route("employees/search/employees.{format}")]
        [Authorize(AuthenticationSchemes = "AzureAD", Roles = "ServicioMedico,PRL,RRHHDesc,RRHHCent")]
        public async Task<IEnumerable<GetEmployeesReport.GetEmployeesReportResponse>> GetAllEmployeesReport([FromRoute]string format, [FromBody]GetEmployeesReport.GetEmployeesReportRequest request) 
            => await Mediator.Send(request).ConfigureAwait(false);

        /// <summary>
        /// Metodo que obtiene los test medicos de un empleado 
        /// </summary>
        /// <param name="idEmpleado">Identificador del empleado</param>
        /// <returns></returns>
        [HttpGet]
        [Route("employee/{idEmpleado}/medicalTest")]
        [ProducesResponseType(typeof(GenericResponse<GetMedicalTest.GetMedicalTestResponse>), 200)]
        [Authorize(AuthenticationSchemes = "AzureAD", Roles = "ServicioMedico")]
        public async Task<IActionResult> GetAllMedicalTest(int idEmpleado) => ResponseHelper.CreateResponse(await Mediator.Send(new GetMedicalTest.GetMedicalTestRequest(idEmpleado)).ConfigureAwait(false));

        /// <summary>
        /// Metodo que obtiene los test medicos de un empleado
        /// </summary>
        /// <param name="idEmpleado">Identificador del empleado</param>
        /// <returns></returns>
        [HttpGet("employee/{idEmpleado}/TemperatureMedition")]
        [ProducesResponseType(typeof(GenericResponse<GetMedicalTest.GetMedicalTestResponse>), 200)]
        [Authorize(AuthenticationSchemes = "AzureAD", Roles = "ServicioMedico")]
        public async Task<IActionResult> GetTemperatureMeditions(int idEmpleado) => ResponseHelper.CreateResponse(await Mediator.Send(new GetTemperatureMedition.GetTemperatureMeditionRequest() { IdEmployee = idEmpleado }).ConfigureAwait(false));

        /// <summary>
        /// Metodo que obtiene los seguimientos medicos de un empleado
        /// </summary>
        /// <param name="idEmpleado">Identificador del empleado</param>
        /// <returns></returns>
        [HttpGet("employee/{idEmpleado}/medicalMonitoring")]
        [ProducesResponseType(typeof(GenericResponse<GetMedicalMonitoring.GetMedicalMonitoringResponse>), 200)]
        [Authorize(AuthenticationSchemes = "AzureAD", Roles = "ServicioMedico")]
        public async Task<IActionResult> GetMedicalMonitoring(int idEmpleado) => ResponseHelper.CreateResponse(await Mediator.Send(new GetMedicalMonitoring.GetMedicalMonitoringRequest(idEmpleado)).ConfigureAwait(false));

        /// <summary>
        /// Metodo que obtiene los seguimientos medicos de un empleado
        /// </summary>
        /// <param name="idEmpleado">Identificador del empleado</param>
        /// <returns></returns>
        [HttpGet("employee/{idEmpleado}/SymptomsInquiryResult")]
        [ProducesResponseType(typeof(GenericResponse<GetSymptomInquiryResult.GetSymptomInquiryResultResponse>), 200)]
        [Authorize(AuthenticationSchemes = "AzureAD", Roles = "ServicioMedico")]
        public async Task<IActionResult> GetSymptomInquiryResult(int idEmpleado) => ResponseHelper.CreateResponse(await Mediator.Send(new GetSymptomInquiryResult.GetSymptomInquiryResultRequest() { IdEmployee = idEmpleado }).ConfigureAwait(false));

        /// <summary>
        /// Metodo que obtiene los ultimos factores de riesgo de un empleado
        /// </summary>
        /// <param name="idEmpleado">Identificador del empleado</param>
        /// <returns></returns>
        [HttpGet("employee/{idEmpleado}/riskfactors")]
        [ProducesResponseType(typeof(GenericResponse<GetRiskFactorsByEmployee.GetRiskFactorsByEmployeeResponse>), 200)]
        [Authorize(AuthenticationSchemes = "AzureAD", Roles = "ServicioMedico")]
        public async Task<IActionResult> GetRiskFactors(int idEmpleado) => ResponseHelper.CreateResponse(await Mediator.Send(new GetRiskFactorsByEmployee.GetRiskFactorsByEmployeeRequest(idEmpleado)).ConfigureAwait(false));

        /// <summary>
        /// Metodo que obtiene los test medicos de un empleado
        /// </summary>
        /// <param name="idEmpleado">Identificador del empleado</param>
        /// <returns></returns>
        [HttpGet]
        [Route("employee/{idEmpleado}/Passport")]
        [ProducesResponseType(typeof(GenericResponse<GetInfoPassport.GetInfoPassportResponse>), 200)]
        [Authorize(AuthenticationSchemes = "AzureAD", Roles = "ServicioMedico,PRL,RRHHDesc,RRHHCent,GestorContratas")]
        public async Task<IActionResult> GetInfoPassport(int idEmpleado) => ResponseHelper.CreateResponse(await Mediator.Send(new GetInfoPassport.GetInfoPassportRequest(idEmpleado)).ConfigureAwait(false));

        /// <summary>
        /// Metodo que obtiene el detalle de un empelado
        /// </summary>
        /// <param name="idEmpleado">Identificador del empleado</param>
        /// <returns></returns>
        [HttpGet]
        [Route("employee/{idEmpleado}")]
        [ProducesResponseType(typeof(GenericResponse<GetDetailEmployee.GetDetailEmployeeResponse>), 200)]
        [Authorize(AuthenticationSchemes = "AzureAD", Roles = "ServicioMedico")]
        public async Task<IActionResult> GetDetailEmployee(int idEmpleado) => ResponseHelper.CreateResponse(await Mediator.Send(new GetDetailEmployee.GetDetailEmployeeRequest(idEmpleado)).ConfigureAwait(false));


        /// <summary>
        /// Metodo que obtiene el detalle de un empleado externo de una contrata
        /// </summary>
        /// <param name="idEmpleado">Identificador del empleado</param>
        /// <param name="idResponsableDirecto">Identificador del responsable que consulta el empleado</param>
        /// <returns></returns>
        [HttpGet]
        [Route("externalEmployees/{idEmpleado}")]
        [ProducesResponseType(typeof(GenericResponse<GetDetailEmployeeExternal.GetDetailEmployeeExternalResponse>), 200)]
        [Authorize(AuthenticationSchemes = "AzureAD", Roles = "GestorContratas")]
        public async Task<IActionResult> GetDetailEmployeeExternal(int idEmpleado) => ResponseHelper.CreateResponse(await Mediator.Send(new GetDetailEmployeeExternal.GetDetailEmployeeExternalRequest(idEmpleado)).ConfigureAwait(false));

        /// <summary>
        /// Metodo que obtiene los empleados externos subordinados
        /// </summary>
        /// <param name="idEmpleado">Identificador del empleado</param>
        /// <returns></returns>
        [HttpPost]
        [Route("externalEmployees/subordinates/search")]
        [ProducesResponseType(typeof(GenericResponse<GetExternalSubordinateEmployees.GetExternalSubordinateEmployeesResponse>), 200)]
        [Authorize(AuthenticationSchemes = "AzureAD", Roles = "GestorContratas")]
        public async Task<IActionResult> GetExternalSubordinateEmployees([FromBody]GetExternalSubordinateEmployees.GetExternalSubordinateEmployeesRequest request) => ResponseHelper.CreateResponse(await Mediator.Send(request).ConfigureAwait(false));

        /// <summary>
        /// Metodo que obtiene los empleados externos subordinados
        /// </summary>
        /// <param name="idEmpleado">Identificador del empleado</param>
        /// <returns></returns>
        [HttpPost]
        [FormatFilter]
        [Route("externalEmployees/subordinates/search/subordinates.{format}")]
        [Authorize(AuthenticationSchemes = "AzureAD", Roles = "GestorContratas")]
        public async Task<IEnumerable<GetExternalSubordinateEmployeesReport.GetExternalSubordinateEmployeesReportResponse>> GetExternalSubordinateEmployeesReport([FromBody]GetExternalSubordinateEmployeesReport.GetExternalSubordinateEmployeesReportRequest request) => await Mediator.Send(request).ConfigureAwait(false);

        #endregion

        #region Delete

        /// <summary>
        /// Metodo que borra los tests rápidos de un empleado
        /// </summary>
        /// <param name="idEmpleado">Identificador del empleado</param>
        /// <param name="idQuickTest">Identificador del test rápido</param>
        /// <returns></returns>
        [HttpDelete("employee/{idEmpleado}/medicalQuickTest/{idQuickTest}")]
        [ProducesResponseType(typeof(GenericResponse<GetMedicalTest.GetMedicalTestResponse>), 200)]
        [Authorize(AuthenticationSchemes = "AzureAD", Roles = "ServicioMedico")]
        public async Task<IActionResult> DeleteQuickTest(int idEmpleado, int idQuickTest) => ResponseHelper.CreateResponse(await Mediator.Send(new DeleteQuickTest.DeleteQuickTestRequest() { IdQuickTest = idQuickTest }).ConfigureAwait(false));

        /// <summary>
        /// Metodo que borra los tests PCR de un empleado
        /// </summary>
        /// <param name="idEmpleado">Identificador del empleado</param>
        /// <param name="idPcrTest">Identificador del test PCR</param>
        /// <returns></returns>
        [HttpDelete("employee/{idEmpleado}/medicalPcrTest/{idPcrTest}")]
        [ProducesResponseType(typeof(GenericResponse<GetMedicalTest.GetMedicalTestResponse>), 200)]
        [Authorize(AuthenticationSchemes = "AzureAD", Roles = "ServicioMedico")]
        public async Task<IActionResult> DeletePcrTest(int idEmpleado, int idPcrTest) => ResponseHelper.CreateResponse(await Mediator.Send(new DeletePcrTest.DeletePcrTestRequest() { IdPcrTest = idPcrTest }).ConfigureAwait(false));

        /// <summary>
        /// Metodo que borra los seguimientos medicos de un empleado
        /// </summary>
        /// <param name="idEmpleado">Identificador del empleado</param>
        /// <param name="idMedicalMonitoring">Identificador del seguimiento médico</param>
        /// <returns></returns>
        [HttpDelete("employee/{idEmpleado}/medicalMonitoring/{idMedicalMonitoring}")]
        [ProducesResponseType(typeof(GenericResponse<GetMedicalMonitoring.GetMedicalMonitoringResponse>), 200)]
        [Authorize(AuthenticationSchemes = "AzureAD", Roles = "ServicioMedico")]
        public async Task<IActionResult> DeleteMedicalMonitoring(int idEmpleado, int idMedicalMonitoring) => ResponseHelper.CreateResponse(await Mediator.Send(new DeleteMedicalMonitoring.DeleteMedicalMonitoringRequest() { IdMedicalMonitoring = idMedicalMonitoring }).ConfigureAwait(false));

        #endregion
    }
}