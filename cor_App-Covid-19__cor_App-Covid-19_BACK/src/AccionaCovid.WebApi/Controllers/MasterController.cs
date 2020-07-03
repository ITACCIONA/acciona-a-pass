using AccionaCovid.Application.Services.Master;
using AccionaCovid.WebApi.Core;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AccionaCovid.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MasterController : MediatorBaseController
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mediator"></param>
        public MasterController(IMediator mediator) : base(mediator)
        {
        }

        #endregion

        #region POST

        /// <summary>
        /// Metodo que vuelca localizaciones
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("locations")]
        [ProducesResponseType(typeof(bool), 200)]
        [Authorize(AuthenticationSchemes = "IdentityServer,AzureAD", Roles = "Administrator")]
        public async Task<IActionResult> BulkLocations(IFormFile locationsCSV) => ResponseHelper.CreateResponse(await Mediator.Send(new BulkLocations.BulkLocationsRequest() { LocationsFile = locationsCSV }).ConfigureAwait(false));

        /// <summary>
        /// Metodo que vuelca departamentos
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("departments")]
        [ProducesResponseType(typeof(bool), 200)]
        [Authorize(AuthenticationSchemes = "IdentityServer,AzureAD", Roles = "Administrator")]
        public async Task<IActionResult> BulkDepartments(IFormFile departmentsCSV) => ResponseHelper.CreateResponse(await Mediator.Send(new BulkDepartments.BulkDepartmentsRequest() { DepartmentsFile = departmentsCSV }).ConfigureAwait(false));

        /// <summary>
        /// Metodo que vuelca empleados
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("employees")]
        [ProducesResponseType(typeof(bool), 200)]
        [Authorize(AuthenticationSchemes = "IdentityServer,AzureAD", Roles = "Administrator")]
        public async Task<IActionResult> BulkEmployees(IFormFile employeesCSV) => ResponseHelper.CreateResponse(await Mediator.Send(new BulkEmployees.BulkEmployeesRequest() { EmployeesFile = employeesCSV }).ConfigureAwait(false));

        /// <summary>
        /// Metodo que vuelca empleados
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("employeesExternal")]
        [ProducesResponseType(typeof(bool), 200)]
        [Authorize(AuthenticationSchemes = "IdentityServer,AzureAD", Roles = "Administrator")]
        public async Task<IActionResult> BulkEmployeesExternal(IFormFile employeesCSV, string origen) => ResponseHelper.CreateResponse(await Mediator.Send(new BulkEmployeesExternal.BulkEmployeesExternalRequest() { EmployeesFile = employeesCSV, Origen=origen }).ConfigureAwait(false));

        // <summary>
        /// Metodo que vuelca Subcontratas
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("works")]
        [ProducesResponseType(typeof(bool), 200)]
        [Authorize(AuthenticationSchemes = "IdentityServer,AzureAD", Roles = "Administrator")]
        public async Task<IActionResult> BulkWorks(IFormFile workCSV) => ResponseHelper.CreateResponse(await Mediator.Send(new BulkWorks.BulkWorksRequest() { WorksFile = workCSV }).ConfigureAwait(false));

        // <summary>
        /// Metodo que vuelca Subcontratas
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("outsourcers")]
        [ProducesResponseType(typeof(bool), 200)]
        [Authorize(AuthenticationSchemes = "IdentityServer,AzureAD", Roles = "Administrator")]
        public async Task<IActionResult> BulkOutSourcers(IFormFile outsourceCSV) => ResponseHelper.CreateResponse(await Mediator.Send(new BulkOutSourcers.BulkOutSourcersRequest() { OutSourcersFile = outsourceCSV }).ConfigureAwait(false));

        // <summary>
        /// Metodo que vuelca Subcontratas
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("workAward")]
        [ProducesResponseType(typeof(bool), 200)]
        [Authorize(AuthenticationSchemes = "IdentityServer,AzureAD", Roles = "Administrator")]
        public async Task<IActionResult> BulkWorkAward(IFormFile workawardCSV) => ResponseHelper.CreateResponse(await Mediator.Send(new BulkWorkAward.BulkWorkAwardRequest() { WorkAwardFile = workawardCSV }).ConfigureAwait(false));

        /// <summary>
        /// Metodo que vuelca roles
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("roles")]
        [ProducesResponseType(typeof(bool), 200)]
        [Authorize(AuthenticationSchemes = "IdentityServer,AzureAD", Roles = "Administrator")]
        public async Task<IActionResult> BulkRoles(IFormFile rolesCSV) => ResponseHelper.CreateResponse(await Mediator.Send(new BulkRoles.BulkRolesRequest() { RolesFile = rolesCSV }).ConfigureAwait(false));

        #endregion

        #region PUT

        /// <summary>
        /// Metodo que actualiza una alerta a leido
        /// </summary>
        /// <param name="idEmpleado">Identificador del empleado</param>
        /// <returns></returns>
        [HttpPut]
        [Route("PassportState")]
        [Authorize(AuthenticationSchemes = "IdentityServer,AzureAD", Roles = "Administrator")]
        [ProducesResponseType(typeof(bool), 200)]
        public async Task<IActionResult> UpdatePassportStateColor([FromBody] UpdatePassportStateColor.UpdatePassportStateColorRequest request) => ResponseHelper.CreateResponse(await Mediator.Send(request).ConfigureAwait(false));

        /// <summary>
        /// Metodo que actualiza una alerta a leido
        /// </summary>
        /// <param name="idEmpleado">Identificador del empleado</param>
        /// <returns></returns>
        [HttpPut]
        [Route("PassportStateDiasValidez")]
        [Authorize(AuthenticationSchemes = "IdentityServer,AzureAD", Roles = "Administrator")]
        [ProducesResponseType(typeof(bool), 200)]
        public async Task<IActionResult> UpdatePassportStateDiasValidez([FromBody] UpdatePassportStateDiasValidez.UpdatePassportStateDiasValidezRequest request) => ResponseHelper.CreateResponse(await Mediator.Send(request).ConfigureAwait(false));

        /// <summary>
        /// Metodo que actualiza una alerta a leido
        /// </summary>
        /// <param name="idEmpleado">Identificador del empleado</param>
        /// <returns></returns>
        [HttpPut]
        [Route("PassportStateName")]
        [Authorize(AuthenticationSchemes = "IdentityServer,AzureAD", Roles = "Administrator")]
        [ProducesResponseType(typeof(bool), 200)]
        public async Task<IActionResult> UpdatePassportStateName([FromBody] UpdatePassportStateName.UpdatePassportStateNameRequest request) => ResponseHelper.CreateResponse(await Mediator.Send(request).ConfigureAwait(false));

        #endregion

        #region GET

        /// <summary>
        /// Metodo que obtiene la matriz de estados
        /// </summary>
        /// <returns></returns>
        [HttpGet("StateMatrix")]
        [ProducesResponseType(typeof(GenericResponse<GetStateMatrix.GetStateMatrixResponse>), 200)]
        //[ResponseCache(Duration = 86400, Location = ResponseCacheLocation.Any, NoStore = false)]
        [Authorize(AuthenticationSchemes = "IdentityServer,AzureAD", Roles = "Administrator")]
        public async Task<IActionResult> GetStateMatrix() => ResponseHelper.CreateResponse(await Mediator.Send(new GetStateMatrix.GetStateMatrixRequest()).ConfigureAwait(false));

        /// <summary>
        /// Throws integration manually
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("integration")]
        [ProducesResponseType(typeof(GenericResponse<IntegrationFileStorage.IntegrationFileStorageResult>), 200)]
        [Authorize(AuthenticationSchemes = "IdentityServer,AzureAD", Roles = "Administrator")]
        public async Task<IActionResult> ThrowIntegrationManually() => ResponseHelper.CreateResponse(await Mediator.Send(new IntegrationFileStorage.IntegrationFileStorageRequest()).ConfigureAwait(false));


        /// <summary>
        /// Metodo que obtiene los estados posibles de un pasaporte
        /// </summary>
        /// <returns></returns>
        [HttpGet("PassportStates")]
        [ProducesResponseType(typeof(GenericResponse<GetPassportStates.GetPassportStatesResponse>), 200)]
        //[ResponseCache(Duration = 86400, Location = ResponseCacheLocation.Any, NoStore = false)]
        [Authorize(AuthenticationSchemes = "IdentityServer,AzureAD", Roles = "Empleado")]
        public async Task<IActionResult> GetAllPassportStatus() => ResponseHelper.CreateResponse(await Mediator.Send(new GetPassportStates.GetPassportStatesRequest()).ConfigureAwait(false));


        /// <summary>
        /// Metodo que obtiene los estados posibles a los que puede transicionar un pasaporte un RRHH
        /// </summary>
        /// <returns></returns>
        [HttpGet("PassportStates/RRHH")]
        [ProducesResponseType(typeof(GenericResponse<GetPassportStates.GetPassportStatesResponse>), 200)]
        //[ResponseCache(Duration = 86400, Location = ResponseCacheLocation.Any, NoStore = false)]
        [Authorize(AuthenticationSchemes = "IdentityServer,AzureAD", Roles = "Empleado")]
        public async Task<IActionResult> GetHRPassportStatus() => ResponseHelper.CreateResponse(await Mediator.Send(new GetHRPassportStates.GetHRPassportStatesRequest()).ConfigureAwait(false));

        /// <summary>
        /// Metodo que obtiene los estados posibles de un pasaporte
        /// </summary>
        /// <returns></returns>
        [HttpGet("PassportStatesColors")]
        [ProducesResponseType(typeof(GenericResponse<GetPassportStateColors.GetPassportStateColorsResponse>), 200)]
        [ResponseCache(Duration = 86400, Location = ResponseCacheLocation.Any, NoStore = false)]
        [Authorize(AuthenticationSchemes = "IdentityServer,AzureAD", Roles = "Empleado")]
        public async Task<IActionResult> GetPassportStateColors() => ResponseHelper.CreateResponse(await Mediator.Send(new GetPassportStateColors.GetPassportStateColorsRequest()).ConfigureAwait(false));

        /// <summary>
        /// Metodo que obtiene los datos de seguimientos medicos
        /// </summary>
        /// <returns></returns>
        [HttpGet("MedicalMonitoring")]
        [ProducesResponseType(typeof(GenericResponse<GetMonitorings.GetMonitoringsResponse>), 200)]
        [ResponseCache(Duration = 86400, Location = ResponseCacheLocation.Any, NoStore = false)]
        [Authorize(AuthenticationSchemes = "IdentityServer,AzureAD", Roles = "Empleado")]
        public async Task<IActionResult> GetMonitorings() => ResponseHelper.CreateResponse(await Mediator.Send(new GetMonitorings.GetMonitoringsRequest()).ConfigureAwait(false));

        /// <summary>
        /// Metodo que obtiene los datos de factores de riesgo
        /// </summary>
        /// <returns></returns>
        [HttpGet("RiskFactors")]
        [ProducesResponseType(typeof(GenericResponse<GetRiskFactors.GetRiskFactorsResponse>), 200)]
        [ResponseCache(Duration = 86400, Location = ResponseCacheLocation.Any, NoStore = false)]
        [Authorize(AuthenticationSchemes = "IdentityServer,AzureAD", Roles = "Empleado")]
        public async Task<IActionResult> GetRiskFactors() => ResponseHelper.CreateResponse(await Mediator.Send(new GetRiskFactors.GetRiskFactorsRequest()).ConfigureAwait(false));

        /// <summary>
        /// Metodo que obtiene los datos de tipos de sintomas
        /// </summary>
        /// <returns></returns>
        [HttpGet("SymptomTypes")]
        [ProducesResponseType(typeof(GenericResponse<GetSymptomTypes.GetSymptomTypesResponse>), 200)]
        [ResponseCache(Duration = 86400, Location = ResponseCacheLocation.Any, NoStore = false)]
        [Authorize(AuthenticationSchemes = "IdentityServer,AzureAD", Roles = "Empleado")]
        public async Task<IActionResult> GetSymptomTypes() => ResponseHelper.CreateResponse(await Mediator.Send(new GetSymptomTypes.GetSymptomTypesRequest()).ConfigureAwait(false));

        /// <summary>
        /// Metodo que obtiene los paises disponibles
        /// </summary>
        /// <returns></returns>
        [HttpGet("countries")]
        [ProducesResponseType(typeof(GenericResponse<GetCountries.GetCountriesResponse>), 200)]
        [ResponseCache(Duration = 21600, Location = ResponseCacheLocation.Any, NoStore = false)]
        [Authorize(AuthenticationSchemes = "IdentityServer,AzureAD", Roles = "Empleado")]
        public async Task<IActionResult> GetCountries() => ResponseHelper.CreateResponse(await Mediator.Send(new GetCountries.GetCountriesRequest()).ConfigureAwait(false));

        /// <summary>
        /// Metodo que obtiene los paises disponibles
        /// </summary>
        /// <returns></returns>
        [HttpGet("departments")]
        [ProducesResponseType(typeof(GenericResponse<GetDepartments.GetDepartmentsResponse>), 200)]
        [ResponseCache(Duration = 21600, Location = ResponseCacheLocation.Any, NoStore = false)]
        [Authorize(AuthenticationSchemes = "IdentityServer,AzureAD", Roles = "Empleado")]
        public async Task<IActionResult> GetDepartments() => ResponseHelper.CreateResponse(await Mediator.Send(new GetDepartments.GetDepartmentsRequest()).ConfigureAwait(false));

        /// <summary>
        /// Metodo que obtiene las divisiones disponibles
        /// </summary>
        /// <returns></returns>
        [HttpGet("divisions")]
        [ProducesResponseType(typeof(GenericResponse<GetDivisions.GetDivisionsResponse>), 200)]
        [ResponseCache(Duration = 21600, Location = ResponseCacheLocation.Any, NoStore = false)]
        [Authorize(AuthenticationSchemes = "IdentityServer,AzureAD", Roles = "Empleado")]
        public async Task<IActionResult> GetDivisions() => ResponseHelper.CreateResponse(await Mediator.Send(new GetDivisions.GetDivisionsRequest()).ConfigureAwait(false));

        /// <summary>
        /// Metodo que obtiene las localizaciones disponibles
        /// </summary>
        /// <returns></returns>
        [HttpGet("locations")]
        [ProducesResponseType(typeof(GenericResponse<GetLocations.GetLocationsResponse>), 200)]
        [ResponseCache(Duration = 21600, Location = ResponseCacheLocation.Any, NoStore = false)]
        [Authorize(AuthenticationSchemes = "IdentityServer,AzureAD", Roles = "Empleado")]
        public async Task<IActionResult> GetLocations(string countryName, int? idRegion, int? idArea) => ResponseHelper.CreateResponse(await Mediator.Send(new GetLocations.GetLocationsRequest()
        {
            CountryName = countryName,
            IdRegion = idRegion,
            IdArea = idArea
        }).ConfigureAwait(false));

        /// <summary>
        /// Metodo que obtiene las ciudades disponibles
        /// </summary>
        /// <returns></returns>
        [HttpGet("cities")]
        [ProducesResponseType(typeof(GenericResponse<GetLocations.GetLocationsResponse>), 200)]
        [ResponseCache(Duration = 21600, Location = ResponseCacheLocation.Any, NoStore = false)]
        [Authorize(AuthenticationSchemes = "IdentityServer,AzureAD", Roles = "Empleado")]
        public async Task<IActionResult> GetCities() => ResponseHelper.CreateResponse(await Mediator.Send(new GetCities.GetCitiesRequest()).ConfigureAwait(false));

        /// <summary>
        /// Metodo que obtiene las ciudades disponibles
        /// </summary>
        /// <returns></returns>
        [HttpGet("employees")]
        [ProducesResponseType(typeof(GenericResponse<GetEmployees.GetMasterEmployeesResponse>), 200)]
        [ResponseCache(Duration = 30, Location = ResponseCacheLocation.Any, NoStore = false)]
        [Authorize(AuthenticationSchemes = "IdentityServer,AzureAD", Roles = "Empleado")]
        public async Task<IActionResult> GetEmployees(string filter) => ResponseHelper.CreateResponse(await Mediator.Send(new GetEmployees.GetEmployeesRequest { Filtro = filter }).ConfigureAwait(false));

        /// <summary>
        /// Metodo que obtiene las tecnologías disponibles
        /// </summary>
        /// <returns></returns>
        [HttpGet("technologies")]
        [ProducesResponseType(typeof(GenericResponse<GetTechnologies.GetTechnologiesResponse>), 200)]
        [ResponseCache(Duration = 21600, Location = ResponseCacheLocation.Any, NoStore = false)]
        [Authorize(AuthenticationSchemes = "IdentityServer,AzureAD", Roles = "Empleado")]
        public async Task<IActionResult> GetTechnologies() => ResponseHelper.CreateResponse(await Mediator.Send(new GetTechnologies.GetTechnologiesRequest()).ConfigureAwait(false));

        /// <summary>
        /// Metodo que obtiene las regiones disponibles
        /// </summary>
        /// <returns></returns>
        [HttpGet("regions")]
        [ProducesResponseType(typeof(GenericResponse<GetRegions.GetRegionsResponse>), 200)]
        [ResponseCache(Duration = 21600, Location = ResponseCacheLocation.Any, NoStore = false)]
        [Authorize(AuthenticationSchemes = "IdentityServer,AzureAD", Roles = "Empleado")]
        public async Task<IActionResult> GetRegions() => ResponseHelper.CreateResponse(await Mediator.Send(new GetRegions.GetRegionsRequest()).ConfigureAwait(false));

        /// <summary>
        /// Metodo que obtiene las areas disponibles
        /// </summary>
        /// <returns></returns>
        [HttpGet("areas")]
        [ProducesResponseType(typeof(GenericResponse<GetRegions.GetRegionsResponse>), 200)]
        [ResponseCache(Duration = 21600, Location = ResponseCacheLocation.Any, NoStore = false)]
        [Authorize(AuthenticationSchemes = "IdentityServer,AzureAD", Roles = "Empleado")]
        public async Task<IActionResult> GetAreas() => ResponseHelper.CreateResponse(await Mediator.Send(new GetAreas.GetAreasRequest()).ConfigureAwait(false));

        #endregion
    }
}