using AccionaCovid.Application.Core;
using AccionaCovid.Application.Resources;
using AccionaCovid.Crosscutting;
using AccionaCovid.Domain.Core;
using AccionaCovid.Domain.Model;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AccionaCovid.Application.Services.MedicalServices
{
    /// <summary>
    /// Servicio que obtiene el detalle de un empleado
    /// </summary>
    public class GetDetailEmployee
    {
        /// <summary>
        /// </summary>
        public class GetDetailEmployeeRequest : IRequest<GetDetailEmployeeResponse>
        {
            public GetDetailEmployeeRequest(int idEmpleado)
            {
                IdEmployee = idEmpleado;
            }

            /// <summary>
            /// Nombre del empleado
            /// </summary>
            public int IdEmployee { get; set; }
        }

        /// <summary>
        /// Validador del request
        /// </summary>
        public class GetDetailEmployeeRequestValidator : AbstractValidator<GetDetailEmployeeRequest>
        {
            /// <summary>
            /// Validaciones a realizar
            /// </summary>
            public GetDetailEmployeeRequestValidator()
            {
                RuleFor(v => v.IdEmployee).Must(p => p > 0).WithFormatMessage(ValidatorsMessages.VALUE_CANNOT_BE_ZERO_OR_NEGATIVE, ValidatorFields.IdEmpleado);
            }
        }

        /// <summary>
        /// Informacion de una lista paginada
        /// </summary>
        public class GetDetailEmployeeResponse
        {
            /// <summary>
            /// Identificador del empleado
            /// </summary>
            public int IdEmpleado { get; set; }

            /// <summary>
            /// Nombre del empleado
            /// </summary>
            public string NombreEmpleado { get; set; }

            /// <summary>
            /// Apellidos del empleado
            /// </summary>
            public string ApellidosEmpleado { get; set; }

            /// <summary>
            /// Iniciales del empleado
            /// </summary>
            public string InicialesEmpleado { get; set; }

            /// <summary>
            /// DNI del empleado
            /// </summary>
            public string DNI { get; set; }

            /// <summary>
            /// Genero del empleado
            /// </summary>
            public string GeneroEmpleado { get; set; }

            /// <summary>
            /// Fecha nacimiento del empleado
            /// </summary>
            public DateTime? FechaNacimiento { get; set; }

            /// <summary>
            /// Edad del empleado
            /// </summary>
            public int EdadEmpleado { get; set; }

            /// <summary>
            /// Numero del empleado
            /// </summary>
            public long NumEmpleado { get; set; }

            /// <summary>
            /// Telefono del empleado
            /// </summary>
            public string TelefonoEmpleado { get; set; }

            /// <summary>
            /// Mail del empleado
            /// </summary>
            public string MailEmpleado { get; set; }

            /// <summary>
            /// Departamento del empleado
            /// </summary>
            public string Departamento { get; set; }

            /// <summary>
            /// Localizacion del empleado
            /// </summary>
            public string NameLocalizacion { get; set; }

            /// <summary>
            /// Pais de la localizacion del empleado
            /// </summary>
            public string Pais { get; set; }

            /// <summary>
            /// Direccion de la localizacion del empleado
            /// </summary>
            public string Direccion1 { get; set; }

            /// <summary>
            /// Ciudad de la localizacion del empleado
            /// </summary>
            public string Ciudad { get; set; }

            /// <summary>
            /// Codigo POstal de la localizacion del empleado
            /// </summary>
            public string CodigoPostal { get; set; }

            /// <summary>
            /// Division del empleado
            /// </summary>
            public string Division { get; set; }

            /// <summary>
            /// Responsable del empleado
            /// </summary>
            public string Responsable { get; set; }
        }

        /// <summary>
        /// Manejador del procesado de un evento
        /// </summary>
        public class GetDetailEmployeeCommandHandler : BaseCommandHandler<GetDetailEmployeeRequest, GetDetailEmployeeResponse>
        {
            /// <summary>
            /// Repositorio a usar.
            /// </summary>
            private readonly IRepository<Empleado> repositoryEmpleado;

            /// <summary>
            /// Constructor
            /// </summary>
            public GetDetailEmployeeCommandHandler(IRepository<Empleado> repositoryEmpleado)
            {
                this.repositoryEmpleado = repositoryEmpleado ?? throw new ArgumentNullException(nameof(repositoryEmpleado));
            }

            /// <summary>
            /// Handle
            /// </summary>
            /// <param name="request"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public override async Task<GetDetailEmployeeResponse> Handle(GetDetailEmployeeRequest request, CancellationToken cancellationToken)
            {
                Empleado empleado = await ValidacionEmpleado(request.IdEmployee).ConfigureAwait(false);

                GetDetailEmployeeResponse response = new GetDetailEmployeeResponse()
                {
                    IdEmpleado = empleado.Id,
                    NombreEmpleado = empleado.Nombre,
                    ApellidosEmpleado = empleado.Apellido,
                    DNI = empleado.Nif,
                    FechaNacimiento = empleado.FechaNacimiento,
                    MailEmpleado = empleado.IdFichaLaboralNavigation?.MailProf,
                    TelefonoEmpleado = empleado.IdFichaLaboralNavigation?.TelefonoCorp,
                    InicialesEmpleado = empleado.Iniciales,
                    EdadEmpleado = empleado.CalcularEdad(),
                    NumEmpleado = empleado.NumEmpleado ?? empleado.Id,
                    Departamento = empleado.IdFichaLaboralNavigation?.IdDepartamentoNavigation?.Nombre,
                    Division = empleado.IdFichaLaboralNavigation?.IdDivisionNavigation?.Nombre,
                    NameLocalizacion = empleado.IdFichaLaboralNavigation?.IdLocalizacionNavigation?.Nombre,
                    Pais = empleado.IdFichaLaboralNavigation?.IdLocalizacionNavigation?.Pais,
                    Ciudad = empleado.IdFichaLaboralNavigation?.IdLocalizacionNavigation?.Ciudad,
                    CodigoPostal = empleado.IdFichaLaboralNavigation?.IdLocalizacionNavigation?.CodigoPostal,
                    Direccion1 = empleado.IdFichaLaboralNavigation?.IdLocalizacionNavigation?.Direccion1,
                    Responsable = "", //TODO: Meter relacion
                    GeneroEmpleado = empleado.Genero != null ? ((Empleado.Gender)empleado.Genero).ToString() : null,
                };

                return response;
            }

            /// <summary>
            /// Metodo que valida si existe el empleado y su ficha
            /// </summary>
            /// <param name="idEmpleado"></param>
            /// <returns></returns>
            private async Task<Empleado> ValidacionEmpleado(int idEmpleado)
            {
                Empleado empleado = await repositoryEmpleado
                    .GetBy(p => p.Id == idEmpleado)
                        .Include(c => c.IdFichaLaboralNavigation)
                            .ThenInclude(c => c.IdDepartamentoNavigation)
                        .Include(c => c.IdFichaLaboralNavigation)
                            .ThenInclude(c => c.IdDivisionNavigation)
                        .Include(c => c.IdFichaLaboralNavigation)
                            .ThenInclude(c => c.IdLocalizacionNavigation)
                    .SingleOrDefaultAsync()
                    .ConfigureAwait(false);

                if (empleado == null)
                {
                    throw new MultiMessageValidationException(new ErrorMessage()
                    {
                        Code = "NOT_FOUND",
                        Message = string.Format(ValidatorsMessages.NOT_FOUND, ValidatorFields.Empleado)
                    });
                }

                return empleado;
            }
        }
    }
}
