using AccionaCovid.Application.Core;
using AccionaCovid.Application.Resources;
using AccionaCovid.Crosscutting;
using AccionaCovid.Domain.Core;
using AccionaCovid.Domain.Model;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AccionaCovid.Application.Services.MedicalServices
{
    /// <summary>
    /// Servicio que obtiene el detalle de un empleado
    /// </summary>
    public class GetDetailEmployeeExternal
    {
        /// <summary>
        /// </summary>
        public class GetDetailEmployeeExternalRequest : IRequest<GetDetailEmployeeExternalResponse>
        {
            public GetDetailEmployeeExternalRequest(int idEmpleado)
            {
                IdEmployee = idEmpleado;
            }

            /// <summary>
            /// id del empleado
            /// </summary>
            public int IdEmployee { get; set; }
        }

        /// <summary>
        /// Validador del request
        /// </summary>
        public class GetDetailEmployeeExternalRequestValidator : AbstractValidator<GetDetailEmployeeExternalRequest>
        {
            /// <summary>
            /// Validaciones a realizar
            /// </summary>
            public GetDetailEmployeeExternalRequestValidator()
            {
                RuleFor(v => v.IdEmployee).Must(p => p > 0).WithFormatMessage(ValidatorsMessages.VALUE_CANNOT_BE_ZERO_OR_NEGATIVE, ValidatorFields.IdEmpleado);
            }
        }

        /// <summary>
        /// Informacion de una lista paginada
        /// </summary>
        public class GetDetailEmployeeExternalResponse
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
            /// DNI del empleado
            /// </summary>
            public string DNI { get; set; }

            /// <summary>
            /// Departamento del empleado
            /// </summary>
            public string Departamento { get; set; }

            /// <summary>
            /// Localizacion del empleado
            /// </summary>
            public string NameLocalizacion { get; set; }

            /// <summary>
            /// Division del empleado
            /// </summary>
            public string Division { get; set; }

            /// <summary>
            /// Responsable del empleado
            /// </summary>
            public string Responsable { get; set; }

            /// <summary>
            /// Responsable del empleado
            /// </summary>
            public string IdResetear { get; set; }
        }

        /// <summary>
        /// Manejador del procesado de un evento
        /// </summary>
        public class GetDetailEmployeeExternalCommandHandler : BaseCommandHandler<GetDetailEmployeeExternalRequest, GetDetailEmployeeExternalResponse>
        {
            /// <summary>
            /// Repositorio a usar.
            /// </summary>
            private readonly IRepository<Empleado> repositoryEmpleado;

            /// <summary>
            /// User info accesssor
            /// </summary>
            private readonly IUserInfoAccesor userInfoAccesor;

            /// <summary>
            /// Constructor
            /// </summary>
            public GetDetailEmployeeExternalCommandHandler(IRepository<Empleado> repositoryEmpleado, IUserInfoAccesor userInfoAccesor)
            {
                this.repositoryEmpleado = repositoryEmpleado ?? throw new ArgumentNullException(nameof(repositoryEmpleado));
                this.userInfoAccesor = userInfoAccesor ?? throw new ArgumentNullException(nameof(userInfoAccesor));
            }

            /// <summary>
            /// Handle
            /// </summary>
            /// <param name="request"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public override async Task<GetDetailEmployeeExternalResponse> Handle(GetDetailEmployeeExternalRequest request, CancellationToken cancellationToken)
            {
                Empleado empleado = await ValidacionEmpleado(request.IdEmployee, userInfoAccesor.IdUser).ConfigureAwait(false);

                GetDetailEmployeeExternalResponse response = new GetDetailEmployeeExternalResponse()
                {
                    IdEmpleado = empleado.Id,
                    NombreEmpleado = empleado.Nombre,
                    ApellidosEmpleado = empleado.Apellido,
                    DNI = empleado.Nif,
                    Departamento = empleado.IdFichaLaboralNavigation?.IdDepartamentoNavigation?.Nombre,
                    Division = empleado.IdFichaLaboralNavigation?.IdDivisionNavigation?.Nombre,
                    NameLocalizacion = empleado.IdFichaLaboralNavigation?.IdLocalizacionNavigation?.Nombre,
                    Responsable = empleado.IdFichaLaboralNavigation?.IdResponsableDirectoNavigation?.NombreCompleto,
                    IdResetear = empleado.AspNetUsers?.FirstOrDefault(u => u.IdEmpleado == empleado.Id)?.Id
                };

                return response;
            }

            /// <summary>
            /// Metodo que valida si existe el empleado y su ficha
            /// </summary>
            /// <param name="idEmpleado"></param>
            /// <returns></returns>
            private async Task<Empleado> ValidacionEmpleado(int idEmpleado, int idResponsable)
            {
                Empleado empleado = await repositoryEmpleado
                    .GetBy(p => p.Id == idEmpleado)
                        .Include(c => c.IdFichaLaboralNavigation)
                            .ThenInclude(c => c.IdResponsableDirectoNavigation)
                        .Include(c => c.IdFichaLaboralNavigation)
                            .ThenInclude(c => c.IdDepartamentoNavigation)
                        .Include(c => c.IdFichaLaboralNavigation)
                            .ThenInclude(c => c.IdDivisionNavigation)
                        .Include(c => c.IdFichaLaboralNavigation)
                            .ThenInclude(c => c.IdLocalizacionNavigation)
                        .Include(c => c.AspNetUsers)
                    .SingleOrDefaultAsync()
                    .ConfigureAwait(false);

                if (empleado == null ||
                    empleado.IdFichaLaboralNavigation?.IsExternal == false ||
                    empleado.IdFichaLaboralNavigation?.IdResponsableDirecto != idResponsable)
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
